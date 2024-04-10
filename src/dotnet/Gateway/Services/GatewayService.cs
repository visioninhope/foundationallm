using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Exceptions;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Gateway.Services
{
    /// <summary>
    /// Implements the FoundationaLLM Gateway service.
    /// </summary>
    /// <param name="options">The options providing the <see cref="GatewayServiceSettings"/> object.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class GatewayService(
        IOptions<GatewayServiceSettings> options,
        ILogger<GatewayService> logger) : IGatewayService
    {
        private readonly GatewayServiceSettings _settings = options.Value;
        private readonly ILogger<GatewayService> _logger = logger;

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Gateway core service is starting.");

            var openAIEndpoints = _settings.AzureOpenAIEndpoints.Split(";");

            await Task.CompletedTask;
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
        public Task<TextEmbeddingResult> StartEmbeddingOperation(TextEmbeddingRequest embeddingRequest) => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<TextEmbeddingResult> GetEmbeddingOperationResult(string operationId) => throw new GatewayException("Invalid attempt", StatusCodes.Status400BadRequest);
    }
}
