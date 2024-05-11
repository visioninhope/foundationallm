using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Gateway;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using FoundationaLLM.SemanticKernel.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Provides context associated with an embedding model deployment.
    /// </summary>
    /// <param name="deployment">The <see cref="AzureOpenAIAccountDeployment"/> object with the details of the model deployment.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
    public class CompletionModelDeploymentContext(
        AzureOpenAIAccountDeployment deployment,
        ILoggerFactory loggerFactory)
    {
        private const int OPENAI_MAX_INPUT_SIZE_TOKENS = 8191;

        private readonly AzureOpenAIAccountDeployment _deployment = deployment;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger<CompletionModelDeploymentContext> _logger = loggerFactory.CreateLogger<CompletionModelDeploymentContext>();
        private List<GatewayCompletionRequest> _inputRequests = [];

        private readonly ICompletionsService _completionsService = new SemanticKernelCompletionService(
                Options.Create(new SemanticKernelCompletionServiceSettings
                {
                    AuthenticationType = AzureOpenAIAuthenticationTypes.AzureIdentity,
                    Endpoint = deployment.AccountEndpoint,
                    DeploymentName = deployment.Name
                }),
                loggerFactory);

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// The cummulated number of tokens for the current token rate window.
        /// </summary>
        private int _tokenRateWindowTokenCount = 0;
        /// <summary>
        /// The cummulated number of requests for the current request rate window.
        /// </summary>
        private int _requestRateWindowRequestCount = 0;
        /// <summary>
        /// The start timestamp of the current token rate window.
        /// </summary>
        private DateTime _tokenRateWindowStart = DateTime.MinValue;
        /// <summary>
        /// The start timestamp of the current request rate window.
        /// </summary>
        private DateTime _requestRateWindowStart = DateTime.MinValue;

        private int _currentRequestTokenCount = 0;

        public bool HasInput =>
            _inputRequests.Count > 0;

        public bool TryAddCompletion(GatewayCompletionRequest request)
        {
            UpdateRateWindows();

            if (_tokenRateWindowTokenCount + request.TokensCount > _deployment.TokenRateLimit
                || _currentRequestTokenCount + request.TokensCount > OPENAI_MAX_INPUT_SIZE_TOKENS)
                // Adding a new text chunk would either push us over to the token rate limit or exceed the maximum input size, so we need to refuse.
                return false;

            if (_requestRateWindowRequestCount == _deployment.RequestRateLimit)
                // We have already reached the allowed number of requests, so we need to refuse.
                return false;

            _inputRequests.Add(request);
            _tokenRateWindowTokenCount += request.TokensCount;
            _currentRequestTokenCount += request.TokensCount;

            return true;
        }

        public async Task<CompletionResult> GetCompletion()
        {
            try
            {
                var gatewayMetrics = new GatewayCompletionRequestMetrics
                {
                    Id = Guid.NewGuid().ToString().ToLower(),
                    AccountName = _deployment.AccountEndpoint,
                    ModelName = _deployment.ModelName,
                    ModelVersion = _deployment.ModelVersion,
                    TokenRateWindowStart = _tokenRateWindowStart,
                    RequestRateWindowStart = _requestRateWindowStart,
                    TokenRateWindowTokenCount = _tokenRateWindowTokenCount,
                    RequestRateWindowRequestCount = _requestRateWindowRequestCount,
                    CurrentRequestTokenCount = _currentRequestTokenCount,
                    CurrentTextChunkCount = _inputRequests.Count
                };

                _logger.LogInformation("Submitting completion request with id {RequestId} and the following metrics: {RequestMetrics}",
                    gatewayMetrics.Id,
                    JsonSerializer.Serialize<GatewayCompletionRequestMetrics>(gatewayMetrics, _jsonSerializerOptions));

                var embeddingResult =
                    await _completionsService.GetCompletionAsync(_inputRequests);                

                if (embeddingResult.Failed)
                    _logger.LogWarning("The text embedding request with id {RequestId} failed with the following error: {ErrorMessage}",
                        gatewayMetrics.Id,
                        embeddingResult.ErrorMessage!);

                return embeddingResult;
            }
            finally
            {
                _requestRateWindowRequestCount++;

                _inputRequests.Clear();
                _currentRequestTokenCount = 0;
            }
        }

        private void UpdateRateWindows()
        {
            var refTime = DateTime.UtcNow;

            if ((refTime - _tokenRateWindowStart).TotalSeconds >= _deployment.TokenRateRenewalPeriod)
            {
                _tokenRateWindowStart = refTime;

                // Reset the rate window token count to the sum of token counts of all current input text chunks.
                _tokenRateWindowTokenCount = _inputRequests.Sum(tc => tc.TokensCount);
            }

            if ((refTime - _requestRateWindowStart).TotalSeconds >= _deployment.RequestRateRenewalPeriod)
            {
                _requestRateWindowStart = refTime;
                _requestRateWindowRequestCount = 0;
            }
        }
    }
}
