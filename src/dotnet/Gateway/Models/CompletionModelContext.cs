using FoundationaLLM.Common.Instrumentation;
using FoundationaLLM.Common.Models.Orchestration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Provides context associated with an embedding model.
    /// </summary>
    /// <param name="operations">The global dictionary of <see cref="CompletionOperationContext"/> objects indexed by operation identifier.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class CompletionModelContext(
        ConcurrentDictionary<string, CompletionOperationContext> operations,
        ILogger<CompletionModelContext> logger)
    {
        private readonly ConcurrentDictionary<string, CompletionOperationContext> _operations = operations;
        private readonly ILogger<CompletionModelContext> _logger = logger;
        private readonly object _syncRoot = new();

        /// <summary>
        /// The name of the embedding model.
        /// </summary>
        public required string ModelName { get; set; }

        /// <summary>
        /// A list of <see cref="CompletionModelDeploymentContext"/> objects providing contexts for the available deployments for the model.
        /// </summary>
        public List<CompletionModelDeploymentContext> DeploymentContexts { get; set; } = [];

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
                        .Where(r => r.Failed)
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
                        .Where(r => !r.InProgress)
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
        public SlidingWindowRateLimiter RequestCount { get; set; }

        public SlidingWindowRateLimiter TokenCount { get; set; }
    }
}
