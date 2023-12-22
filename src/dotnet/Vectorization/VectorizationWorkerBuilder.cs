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
        private IConfigurationSection? _queuesConfiguration;
        private IVectorizationStateService? _stateService;
        private CancellationToken _cancellationToken = default(CancellationToken);
        private ILoggerFactory? _loggerFactory;

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

            ValidateAndSetQueuesConfiguration();

            var requestSourceServices = _settings!.RequestSources!
                .Select(rs => GetRequestSourceService(
                    rs,
                    _settings.QueuingEngine))
                .ToDictionary(rs => rs.SourceName);

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

            _settings = settings;
            return this;
        }

        public VectorizationWorkerBuilder WithQueuesConfiguration(IConfigurationSection queuesConfiguration)
        {
            _queuesConfiguration = queuesConfiguration;
            return this;
        }

        public VectorizationWorkerBuilder WithCancellationToken(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return this;
        }

        public VectorizationWorkerBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        private void ValidateSettings(VectorizationWorkerSettings settings)
        {
            if (
                settings == null 
                || settings.RequestManagers == null
                || settings.RequestManagers.Count == 0
                || settings.RequestSources == null
                || settings.RequestSources.Count == 0)
                throw new ArgumentNullException(nameof(settings));

            foreach (var rs in settings.RequestSources)
                if (!VectorizationSteps.ValidateStepName(rs.Name))
                    throw new VectorizationException("Configuration error: invalid request source name in RequestSources.");

            foreach (var rm in settings.RequestManagers)
            {
                if (!VectorizationSteps.ValidateStepName(rm.RequestSourceName))
                    throw new VectorizationException("Configuration error: invalid request source name in RequestManagers.");

                if (!settings.RequestSources.Exists(rs => rs.Name.CompareTo(rm.RequestSourceName) == 0))
                    throw new VectorizationException($"Configuration error: RequestManagers references request source [{rm.RequestSourceName}] which is not configured.");
            }
        }

        private void ValidateAndSetQueuesConfiguration()
        {
            if (_settings == null)
                throw new VectorizationException("The validation of queues configuration can only be performed after providing the settings.");

            if (_settings.QueuingEngine == VectorizationQueuing.None)
                return;

            if (_queuesConfiguration == null
                || _queuesConfiguration.GetChildren().Count() == 0)
                throw new VectorizationException("The queues configuration is empty.");

            foreach (var rs in _settings.RequestSources!)
            {
                var connectionString = _queuesConfiguration[rs.ConnectionConfigurationName];
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new VectorizationException($"The configuration setting [{rs.ConnectionConfigurationName}] was not found.");
                rs.ConnectionString = connectionString;
            }
        }

        private IRequestSourceService GetRequestSourceService(RequestSourceServiceSettings settings, VectorizationQueuing queuing)
        {
            switch (queuing)
            {
                case VectorizationQueuing.None:
                    return new MemoryRequestSourceService(
                        settings,
                        _loggerFactory!.CreateLogger<MemoryRequestSourceService>());
                case VectorizationQueuing.AzureStorageQueue:
                    return new StorageQueueRequestSourceService(
                        settings,
                        _loggerFactory!.CreateLogger<StorageQueueRequestSourceService>());
                default:
                    throw new VectorizationException($"The vectorization queuing mechanism [{queuing}] is not supported.");
            }
        }
    }
}
