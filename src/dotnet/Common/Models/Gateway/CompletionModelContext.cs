using FoundationaLLM.Common.Instrumentation;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Gateway.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FoundationaLLM.Common.Models.Gateway
{
    /// <summary>
    /// Provides context associated with an embedding model.
    /// </summary>
    /// <param name="operations">The global dictionary of <see cref="CompletionOperationContext"/> objects indexed by operation identifier.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class CompletionModelContext : ModelContext
    {
        private readonly ConcurrentDictionary<string, CompletionOperationContext> _operations;
        private readonly ILogger<CompletionModelContext> _logger;
        private readonly object _syncRoot = new();

        public CompletionModelContext(
            AzureOpenAIAccountDeployment deployment,
            ConcurrentDictionary<string, CompletionOperationContext> operations,
            ILogger<CompletionModelContext> logger)
        {
            _logger = logger;
            _operations = operations;

            ModelName = deployment.ModelName;
            RequestCount = new SlidingWindowRateLimiter(deployment.RequestRateLimit, deployment.RequestRateRenewalPeriod, "completions.request.count", this);
            TokenCount = new SlidingWindowRateLimiter(deployment.TokenRateLimit / 6, deployment.TokenRateRenewalPeriod / 6, "completions.token.count", this);
        }

        public CompletionModelContext(
            string modelName,
            int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod,
            ConcurrentDictionary<string, CompletionOperationContext> operations,
            ILogger<CompletionModelContext> logger)
        {
            _logger = logger;
            _operations = operations;

            ModelName = modelName;
            RequestCount = new SlidingWindowRateLimiter(requestRateLimit, requestRateRenewalPeriod, "completions.request.count", this);
            TokenCount = new SlidingWindowRateLimiter(tokenRateLimit / 6, tokenRateRenewalPeriod / 6, "completions.token.count", this);
        }

        /// <summary>
        /// A list of <see cref="CompletionModelDeploymentContextBase"/> objects providing contexts for the available deployments for the model.
        /// </summary>
        public List<CompletionModelDeploymentContextBase> DeploymentContexts { get; set; } = [];

        /// <summary>
        /// The list of active embedding operation identifiers.
        /// </summary>
        private readonly List<string> _operationIds = [];

        public void AddOperationContext(CompletionOperationContext completionOperationContext)
        {
            _operations.AddOrUpdate(
                completionOperationContext.Result.OperationId!,
                completionOperationContext,
            (k, v) => v);

            lock (_syncRoot)
            {
                _operationIds.Add(completionOperationContext.Result.OperationId!);
            }
        }

        /// <summary>
        /// Processes completion operations in a continuous loop.
        /// </summary>
        /// <param name="cancellationToken">Notifies that the operation must be cancelled.</param>
        /// <returns></returns>
        public async Task ProcessOperations(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Completions operations processing started for the {ModelName} model.", ModelName);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;

                try
                {
                    lock (_syncRoot)
                    {
                        var currentDeploymentContextIndex = 0;
                        var capacityReached = false;

                        foreach (var operationId in _operationIds)
                        {
                            if (_operations.TryGetValue(operationId, out var operationContext)
                                && operationContext.Result.InProgress)

                                
                                if (!DeploymentContexts[currentDeploymentContextIndex].TryAddCompletion(operationContext.CompletionRequest))
                                {
                                    currentDeploymentContextIndex++;
                                    if (currentDeploymentContextIndex == DeploymentContexts.Count)
                                    {
                                        // We're at capacity.
                                        capacityReached = true;
                                        break;
                                    }
                                }
                                

                            if (capacityReached) break;
                        }
                    }

                    // Use all available deployments to get embeddings for the input text chunks.
                    var results = await Task.WhenAll(DeploymentContexts
                        .Where(dc => dc.HasInput) 
                        .Select(dc => dc.GetCompletion()));

                    // Record all failed operations
                    foreach (var failedOperation in results
                        .Where(r => r != null && r.Failed)
                        .GroupBy(x => x.OperationId)
                        .Select(g => new
                        {
                            OperationId = g.Key
                        }))
                    {
                        _logger.LogError($"An error occured in completion operation {failedOperation.OperationId}. ");
                    }

                    // Set the response for all successful operations.
                    foreach (var successfulOperation in results
                        .Where(r => r != null && !r.InProgress)
                        .GroupBy(x => x.OperationId)
                        .Select(r => new {
                            OperationId = r.Key,
                            Result = r.Select(tc => tc.Result).First()
                        }))
                    {
                        _operations[successfulOperation.OperationId!].SetCompletion(successfulOperation.Result);

                        lock (_syncRoot)
                        {
                            if (!_operations[successfulOperation.OperationId!].Result.InProgress)
                                _operationIds.Remove(successfulOperation.OperationId!);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while attempting to execute a processing cycle.");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
