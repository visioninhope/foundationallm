using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Services;
using FoundationaLLM.Vectorization.Services.RequestSources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FoundationaLLM.Vectorization
{
    public class VectorizationWorkerBuilder
    {
        private VectorizationWorkerSettings? _settings;
        private IVectorizationStateService? _stateService;
        private CancellationToken _cancellationToken = default;
        private ILoggerFactory? _loggerFactory;

        private readonly RequestSourcesBuilder _requestSourcesBuilder = new();

        public VectorizationWorkerBuilder()
        {
        }

        public VectorizationWorker Build()
        {
            if (_stateService == null)
                throw new VectorizationException("Cannot build a vectorization worker without a valid vectorization state service.");

            if (_settings == null)
                throw new VectorizationException("Cannot build a vectorization worker without valid settings.");

            if (_loggerFactory == null)
                throw new VectorizationException("Cannot build a vectorization worker without a valid logger factory.");

            var requestSourceServices = _requestSourcesBuilder.Build();

            var requestManagerServices = _settings!.RequestManagers!
                .Select(rm => new RequestManagerService(
                    rm,
                    requestSourceServices,
                    _stateService,
                    _loggerFactory!.CreateLogger<RequestManagerService>(),
                    _cancellationToken))
                .ToList();

            var vectorizationWorker = new VectorizationWorker(
                _stateService,
                requestSourceServices,
                requestManagerServices,
                _loggerFactory!.CreateLogger<VectorizationWorker>(),
                _cancellationToken);

            return vectorizationWorker;
        }

        public VectorizationWorkerBuilder WithStateService(IVectorizationStateService stateService)
        {
            _stateService = stateService;
            return this;
        }

        public VectorizationWorkerBuilder WithSettings(VectorizationWorkerSettings settings) 
        {
            ValidateSettings(settings);

            _requestSourcesBuilder.WithSettings(settings!.RequestSources);
            _requestSourcesBuilder.WithQueuing(settings!.QueuingEngine);

            _settings = settings;
            return this;
        }

        public VectorizationWorkerBuilder WithCancellationToken(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return this;
        }

        public VectorizationWorkerBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _requestSourcesBuilder.WithLoggerFactory(loggerFactory);

            _loggerFactory = loggerFactory;
            return this;
        }

        public VectorizationWorkerBuilder WithQueuesConfiguration(IConfigurationSection queuesConfiguration)
        {
            _requestSourcesBuilder.WithQueuesConfiguration(queuesConfiguration);
            return this;
        }

        private static void ValidateSettings(VectorizationWorkerSettings settings)
        {
            if (
                settings == null 
                || settings.RequestManagers == null
                || settings.RequestManagers.Count == 0
                || settings.RequestSources == null)
                throw new ArgumentNullException(nameof(settings));

            foreach (var rm in settings.RequestManagers)
            {
                if (!VectorizationSteps.ValidateStepName(rm.RequestSourceName))
                    throw new VectorizationException("Configuration error: invalid request source name in RequestManagers.");

                if (!settings.RequestSources.Exists(rs => rs.Name.CompareTo(rm.RequestSourceName) == 0))
                    throw new VectorizationException($"Configuration error: RequestManagers references request source [{rm.RequestSourceName}] which is not configured.");
            }
        }
    }
}
