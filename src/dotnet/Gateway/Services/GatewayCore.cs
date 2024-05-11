using FoundationaLLM.Common.Instrumentation;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Exceptions;
using FoundationaLLM.Gateway.Instrumentation;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models;
using FoundationaLLM.Gateway.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace FoundationaLLM.Gateway.Services
{
    /// <summary>
    /// Implements the FoundationaLLM Gateway service.
    /// </summary>
    /// <param name="armService">The <see cref="IAzureResourceManagerService"/> instance providing Azure Resource Manager services.</param>
    /// <param name="options">The options providing the <see cref="GatewayCoreSettings"/> object.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
    public class GatewayCore(
        IAzureResourceManagerService armService,
        IOptions<GatewayCoreSettings> options,
        ILoggerFactory loggerFactory,
        GatewayInstrumentation gatewayInstrumentation) : IGatewayCore
    {
        private readonly IAzureResourceManagerService _armService = armService;
        private readonly GatewayCoreSettings _settings = options.Value;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger<GatewayCore> _logger = loggerFactory.CreateLogger<GatewayCore>();
        private GatewayInstrumentation _gatewayInstrumentation = gatewayInstrumentation;

        private bool _initialized = false;

        private Dictionary<string, AzureOpenAIAccount> _azureOpenAIAccounts = [];

        private Dictionary<string, EmbeddingModelContext> _embeddingModels = [];
        private Dictionary<string, CompletionModelContext> _completionModels = [];

        private ConcurrentDictionary<string, EmbeddingOperationContext> _embeddingOperations = [];
        private ConcurrentDictionary<string, CompletionOperationContext> _completionOperations = [];

        private Dictionary<string, SlidingWindowRateLimiter> _modelLimits = [];

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Gateway core service is starting.");

            try
            {
                var openAIAccounts = _settings.AzureOpenAIAccounts.Split(";");
                foreach (var openAIAccount in openAIAccounts)
                {
                    _logger.LogInformation("Loading properties for the Azure OpenAI account with resource id {AccountResourceId}.", openAIAccount);

                    try
                    {
                        var accountProperties = await _armService.GetOpenAIAccountProperties(openAIAccount);
                        _azureOpenAIAccounts.Add(accountProperties.Name, accountProperties);

                        foreach (var deployment in accountProperties.Deployments)
                        {
                            if (deployment.CanDoEmbeddings)
                            {
                                var embeddingModelContext = new EmbeddingModelDeploymentContext(
                                    deployment,
                                    _loggerFactory);

                                if (!_embeddingModels.ContainsKey(deployment.ModelName))
                                    _embeddingModels[deployment.ModelName] = new EmbeddingModelContext(
                                        _embeddingOperations,
                                        _loggerFactory.CreateLogger<EmbeddingModelContext>())
                                    {
                                        ModelName = deployment.ModelName,
                                        DeploymentContexts = [embeddingModelContext],
                                        RequestCount = new SlidingWindowRateLimiter(deployment.RequestRateLimit, deployment.RequestRateRenewalPeriod, "embeddings.request.count"),
                                        TokenCount = new SlidingWindowRateLimiter(deployment.TokenRateLimit / 6, deployment.TokenRateRenewalPeriod / 6, "embeddings.token.count")
                                    };
                                else
                                    _embeddingModels[deployment.ModelName].DeploymentContexts.Add(embeddingModelContext);

                                //_gatewayInstrumentation.EmbeddingModels.Add(deployment.ModelName, _embeddingModels[deployment.ModelName]);

                                //_gatewayInstrumentation.AddEmbeddingModel(_embeddingModels[deployment.ModelName]);
                            }

                            if (deployment.CanDoCompletions)
                            {
                                var modelContext = new CompletionModelDeploymentContext(
                                    deployment,
                                    _loggerFactory);

                                if (!_completionModels.ContainsKey(deployment.ModelName))
                                    _completionModels[deployment.ModelName] = new CompletionModelContext(
                                        _completionOperations,
                                        _loggerFactory.CreateLogger<CompletionModelContext>())
                                    {
                                        ModelName = deployment.ModelName,
                                        DeploymentContexts = [modelContext],
                                        RequestCount = new SlidingWindowRateLimiter(deployment.RequestRateLimit, deployment.RequestRateRenewalPeriod, "completions.request.count"),
                                        TokenCount = new SlidingWindowRateLimiter(deployment.TokenRateLimit / 6, deployment.TokenRateRenewalPeriod / 6, "completions.token.count")
                                    };
                                else
                                    _completionModels[deployment.ModelName].DeploymentContexts.Add(modelContext);

                                //_gatewayInstrumentation.EmbeddingModels.Add(deployment.ModelName, _embeddingModels[deployment.ModelName]);

                                //_gatewayInstrumentation.AddEmbeddingModel(_embeddingModels[deployment.ModelName]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "There was an error while loading the properties for the Azure OpenAI account with resource id {AccountResourceId}.", openAIAccount);
                    }
                }

                _initialized = true;
                _logger.LogInformation("The Gateway core service started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The Gateway core did not start successfully due to an error.");
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Gateway core service is stopping.");
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            _logger.LogInformation("The Gateway core service is executing.");

            var modelTasks1 = _embeddingModels.Values
                .Select(em => Task.Run(() => em.ProcessOperations(cancellationToken)))
                .ToArray();

            var modelTasks2 = _completionModels.Values
                .Select(em => Task.Run(() => em.ProcessOperations(cancellationToken)))
                .ToArray();

            var allTask = modelTasks1.Concat(modelTasks2).ToArray();

            await Task.WhenAll(allTask);
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> StartEmbeddingOperation(TextEmbeddingRequest embeddingRequest)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            if (!_embeddingModels.TryGetValue(embeddingRequest.EmbeddingModelName, out var embeddingModel))
                throw new GatewayException("The requested embedding model is not available.", StatusCodes.Status404NotFound);

            var operationId = Guid.NewGuid().ToString().ToLower();

            var embeddingOperationContext = new EmbeddingOperationContext
            {
                InputTextChunks = embeddingRequest.TextChunks.Select(tc => new TextChunk
                {
                    OperationId = operationId,
                    Position = tc.Position,
                    Content = tc.Content,
                    TokensCount = tc.TokensCount
                }).ToList(),
                Result = new TextEmbeddingResult
                {
                    InProgress = true,
                    OperationId = operationId,
                    TextChunks = embeddingRequest.TextChunks.Select(tc => new TextChunk
                    {
                        Position = tc.Position
                    }).ToList(),
                    TokenCount = 0
                }
            };

            embeddingModel.AddOperationContext(embeddingOperationContext);

            return await Task.FromResult(
                new TextEmbeddingResult
                {
                    InProgress = true,
                    OperationId = embeddingOperationContext.Result.OperationId
                });
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingOperationResult(string operationId)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            if (!_embeddingOperations.TryGetValue(operationId, out var operationContext))
                throw new GatewayException("The operation identifier was not found.", StatusCodes.Status404NotFound);

            if (operationContext.Result.Failed)
                return await Task.FromResult(new TextEmbeddingResult
                {
                    InProgress = false,
                    Failed = true,
                    ErrorMessage = operationContext.Result.ErrorMessage,
                    OperationId = operationId
                });
            else if (operationContext.Result.InProgress)
                return await Task.FromResult(new TextEmbeddingResult
                {
                    InProgress = true,
                    OperationId = operationId
                });
            else
                return await Task.FromResult(operationContext.Result);
        }

        public async Task<CompletionResult> StartCompletionOperation(GatewayCompletionRequest completionRequest)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            if (!_completionModels.TryGetValue(completionRequest.Settings.ModelParameters["model_name"].ToString(), out var model))
                throw new GatewayException("The requested embedding model is not available.", StatusCodes.Status404NotFound);

            var operationId = Guid.NewGuid().ToString().ToLower();

            completionRequest.OperationId = operationId;

            var embeddingOperationContext = new CompletionOperationContext
            {
                CompletionRequest = completionRequest,
                Result = new CompletionResult
                {
                    InProgress = true,
                    OperationId = operationId,
                    TokenCount = 0
                }
            };

            model.AddOperationContext(embeddingOperationContext);

            return await Task.FromResult(
                new CompletionResult
                {
                    InProgress = true,
                    OperationId = embeddingOperationContext.Result.OperationId
                });
        }

        public async Task<CompletionResult> GetCompletionOperationResult(string operationId)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            if (!_completionOperations.TryGetValue(operationId, out var operationContext))
                throw new GatewayException("The operation identifier was not found.", StatusCodes.Status404NotFound);

            if (operationContext.Result.Failed)
                return await Task.FromResult(new CompletionResult
                {
                    InProgress = false,
                    Failed = true,
                    ErrorMessage = operationContext.Result.ErrorMessage,
                    OperationId = operationId
                });
            else if (operationContext.Result.InProgress)
                return await Task.FromResult(new CompletionResult
                {
                    InProgress = true,
                    OperationId = operationId
                });
            else
                return await Task.FromResult(operationContext.Result);
        }

        public async Task<bool> AddModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod)
        {
            EmbeddingModelContext model;

            if (!_gatewayInstrumentation.EmbeddingModels.TryGetValue(modelId, out model))
            {
                model = new EmbeddingModelContext(_embeddingOperations,
                                        _loggerFactory.CreateLogger<EmbeddingModelContext>())
                {
                    ModelName = modelId,
                    DeploymentContexts = [],
                    RequestCount = new SlidingWindowRateLimiter(requestRateLimit, requestRateRenewalPeriod, "embeddings.request.count"),
                    TokenCount = new SlidingWindowRateLimiter(tokenRateLimit / 6, tokenRateRenewalPeriod / 6, "embeddings.token.count")
                };

                _gatewayInstrumentation.EmbeddingModels.Add(modelId, model);
            }

            return true;
        }


        public async Task<bool> TryConsume(string modelId, int tokenCount)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            EmbeddingModelContext model;

            if ( !_gatewayInstrumentation.EmbeddingModels.TryGetValue(modelId, out model))
            {
                throw new Exception("Model was not found, call AddModel first.");
            }
            
            bool requestValid = model.RequestCount.TryConsume(1);
            bool tokenValid = model.TokenCount.TryConsume(tokenCount);

            if ( requestValid && tokenValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
