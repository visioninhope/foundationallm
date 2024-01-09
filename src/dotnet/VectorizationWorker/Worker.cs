
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Vectorization.Worker
{
    /// <summary>
    /// The background service used to run the worker.
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly IVectorizationStateService _stateService;
        private readonly VectorizationWorkerSettings _settings;
        private readonly IConfigurationSection _queuesConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Creates a new instance of the worker.
        /// </summary>
        /// <param name="stateService">The <see cref="IVectorizationStateService"/> used to manage the vectorization state.</param>
        /// <param name="settings">The <see cref="VectorizationWorkerSettings"/> options holding the vectorization worker settings.</param>
        /// <param name="queuesConfiguration">The <see cref="IConfigurationSection"/> containing the settings for the queues.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers in child objects.</param>
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
