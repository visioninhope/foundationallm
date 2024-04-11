using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Gateway.Exceptions;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models;
using FoundationaLLM.Gateway.Models.Configuration;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using FoundationaLLM.SemanticKernel.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        ILoggerFactory loggerFactory) : IGatewayCore
    {
        private readonly IAzureResourceManagerService _armService = armService;
        private readonly GatewayCoreSettings _settings = options.Value;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger<GatewayCore> _logger = loggerFactory.CreateLogger<GatewayCore>();

        private bool _initialized = false;

        private Dictionary<string, AzureOpenAIAccount> _azureOpenAIAccounts = [];
        private Dictionary<string, List<EmbeddingModelContext>> _azureOpenAIEmbeddingModels = [];

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
                            if (deployment.Capabilities!.ContainsKey("embeddings")
                                && deployment.Capabilities["embeddings"] == "true")
                            {
                                var embeddingModelContext = new EmbeddingModelContext
                                {
                                    Deployment = deployment,
                                    TextEmbeddingService = CreateTextEmbeddingService(deployment.AccountEndpoint, deployment.Name)
                                };

                                if (!_azureOpenAIEmbeddingModels.ContainsKey(deployment.ModelName))
                                    _azureOpenAIEmbeddingModels[deployment.ModelName] = [embeddingModelContext];
                                else
                                    _azureOpenAIEmbeddingModels[deployment.ModelName].Add(embeddingModelContext);
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
            _logger.LogInformation("The Gateway core service is executing.");

            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> StartEmbeddingOperation(TextEmbeddingRequest embeddingRequest)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");

            try
            {
                var embeddingService = SelectTextEmbeddingService(embeddingRequest.EmbeddingModelName);

                var result = await embeddingService.GetEmbeddingsAsync(embeddingRequest.TextChunks);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The embedding operation could not be started.");
                throw new GatewayException("The embedding operation could not be started.");
            }
        }

        /// <inheritdoc/>
        public Task<TextEmbeddingResult> GetEmbeddingOperationResult(string operationId)
        {
            if (!_initialized)
                throw new GatewayException("The Gateway service is not initialized.");
            return null;
        }

        private ITextEmbeddingService CreateTextEmbeddingService(string endpoint, string deploymentName)
            => new SemanticKernelTextEmbeddingService(
                Options.Create<SemanticKernelTextEmbeddingServiceSettings>(new SemanticKernelTextEmbeddingServiceSettings
                {
                    AuthenticationType = AzureOpenAIAuthenticationTypes.AzureIdentity,
                    Endpoint = endpoint,
                    DeploymentName = deploymentName
                }),
                _loggerFactory.CreateLogger<SemanticKernelTextEmbeddingService>());

        private ITextEmbeddingService SelectTextEmbeddingService(string modelName)
        {
            if (!_azureOpenAIEmbeddingModels.TryGetValue(modelName, out var modelContext)
                || modelContext.Count == 0)
                throw new GatewayException("Model not found.", StatusCodes.Status404NotFound);

            return modelContext.First().TextEmbeddingService;
        }
    }
}
