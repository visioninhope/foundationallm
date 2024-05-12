using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Gateway;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Gateway.Instrumentation;
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
    public class EmbeddingModelDeploymentContext(
        AzureOpenAIAccountDeployment deployment,
        ILoggerFactory loggerFactory,
        GatewayInstrumentation gatewayInstrumentation) : EmbeddingModelDeploymentContextBase(deployment, loggerFactory)
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger<EmbeddingModelDeploymentContext> _logger = loggerFactory.CreateLogger<EmbeddingModelDeploymentContext>();
        
        private readonly ITextEmbeddingService _textEmbeddingService = new SemanticKernelTextEmbeddingService(
                Options.Create(new SemanticKernelTextEmbeddingServiceSettings
                {
                    AuthenticationType = AzureOpenAIAuthenticationTypes.AzureIdentity,
                    Endpoint = deployment.AccountEndpoint,
                    DeploymentName = deployment.Name
                }),
                loggerFactory,
                gatewayInstrumentation);


        override public async Task<TextEmbeddingResult> GetEmbeddingsForInputTextChunks()
        {
            try
            {
                var gatewayMetrics = new GatewayTextEmbeddingRequestMetrics
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
                    CurrentTextChunkCount = _inputTextChunks.Count,
                    OperationsDetails = _inputTextChunks
                        .GroupBy(tc => tc.OperationId!)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(tc => tc.Position).ToList())
                };

                _logger.LogInformation("Submitting text embedding request with id {RequestId} and the following metrics: {RequestMetrics}",
                    gatewayMetrics.Id,
                    JsonSerializer.Serialize<GatewayTextEmbeddingRequestMetrics>(gatewayMetrics, _jsonSerializerOptions));

                var embeddingResult =
                    await _textEmbeddingService.GetEmbeddingsAsync(_inputTextChunks);

                if (embeddingResult.Failed)
                    _logger.LogWarning("The text embedding request with id {RequestId} failed with the following error: {ErrorMessage}",
                        gatewayMetrics.Id,
                        embeddingResult.ErrorMessage!);

                return embeddingResult;
            }
            finally
            {
                _requestRateWindowRequestCount++;

                _inputTextChunks.Clear();
                _currentRequestTokenCount = 0;
            }
        }

        override protected void UpdateRateWindows()
        {
            var refTime = DateTime.UtcNow;

            if ((refTime - _tokenRateWindowStart).TotalSeconds >= _deployment.TokenRateRenewalPeriod)
            {
                _tokenRateWindowStart = refTime;

                // Reset the rate window token count to the sum of token counts of all current input text chunks.
                _tokenRateWindowTokenCount = _inputTextChunks.Sum(tc => tc.TokensCount);
            }

            if ((refTime - _requestRateWindowStart).TotalSeconds >= _deployment.RequestRateRenewalPeriod)
            {
                _requestRateWindowStart = refTime;
                _requestRateWindowRequestCount = 0;
            }
        }
    }
}
