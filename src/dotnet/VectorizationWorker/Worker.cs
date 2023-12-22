
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Vectorization.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IVectorizationStateService _stateService;
        private readonly VectorizationWorkerSettings _settings;
        private readonly IConfigurationSection _queuesConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        public Worker(
            IVectorizationStateService stateService,
            IOptions<VectorizationWorkerSettings> settings,
            IConfigurationSection queuesConfiguration,
            ILoggerFactory loggerFactory)
        {
            _stateService = stateService;
            _settings = settings.Value;
            _queuesConfiguration = queuesConfiguration;
            _loggerFactory = loggerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var vectorizationWorker = new VectorizationWorkerBuilder()
                .WithStateService(_stateService)
                .WithSettings(_settings)
                .WithQueuesConfiguration(_queuesConfiguration)
                .WithLoggerFactory(_loggerFactory)
                .WithCancellationToken(stoppingToken)
                .Build();

            await vectorizationWorker.Run();
        }
    }
}
