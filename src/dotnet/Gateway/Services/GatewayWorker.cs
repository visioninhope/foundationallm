using FoundationaLLM.Gateway.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Gateway.Services
{
    /// <summary>
    /// Background worker used to execute the Gateway service.
    /// </summary>
    /// <param name="gatewayService">The <see cref="IGatewayService"/> providing the gateway functionalities.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class GatewayWorker(
        IGatewayService gatewayService,
        ILogger<GatewayWorker> logger) : BackgroundService
    {
        private readonly IGatewayService _gatewayService = gatewayService;
        private readonly ILogger<GatewayWorker> _logger = logger;

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("The Gateway worker is starting up the Gateway service.");

            try
            {
                await _gatewayService.StartAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The Gateway service was not able to start.");
            }

            _logger.LogInformation("The Gateway worker is preparing to execute the Gateway service.");
            await _gatewayService.ExecuteAsync(stoppingToken);
        }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Gateway worker is stopping the Gateway service.");
            await _gatewayService.StopAsync(cancellationToken);
        }
    }
}
