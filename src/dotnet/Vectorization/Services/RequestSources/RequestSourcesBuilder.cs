using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class RequestSourcesBuilder
    {
        private List<RequestSourceServiceSettings>? _settings;
        private VectorizationQueuing _queuing;
        private ILoggerFactory? _loggerFactory;
        private IConfigurationSection? _queuesConfiguration;

        public RequestSourcesBuilder()
        {
        }

        public Dictionary<string, IRequestSourceService> Build()
        {
            if (_settings == null)
                throw new VectorizationException("Cannot build a dictionary of request sources without valid settings.");

            if (_loggerFactory == null)
                throw new VectorizationException("Cannot build a dictionary of request sources without a valid logger factory.");

            ValidateAndSetQueuesConfiguration();

            return _settings!
                .Select(rs => GetRequestSourceService(
                    rs,
                    _queuing))
                .ToDictionary(rs => rs.SourceName);
        }

        public RequestSourcesBuilder WithSettings(List<RequestSourceServiceSettings>? settings)
        {
            ValidateSettings(settings);

            _settings = settings;
            return this;
        }

        public RequestSourcesBuilder WithQueuing(VectorizationQueuing queuing)
        {
            _queuing = queuing;
            return this;
        }

        public RequestSourcesBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public RequestSourcesBuilder WithQueuesConfiguration(IConfigurationSection queuesConfiguration)
        {
            _queuesConfiguration = queuesConfiguration;
            return this;
        }

        private static void ValidateSettings(List<RequestSourceServiceSettings>? settings)
        {
            if (
                settings == null
                || settings.Count == 0)
                throw new ArgumentNullException(nameof(settings));

            foreach (var rs in settings)
                if (!VectorizationSteps.ValidateStepName(rs.Name))
                    throw new VectorizationException("Configuration error: invalid request source name in RequestSources.");
        }

        private void ValidateAndSetQueuesConfiguration()
        {
            if (_settings == null)
                throw new VectorizationException("The validation of queues configuration can only be performed after providing the settings.");

            if (_queuing == VectorizationQueuing.None)
                return;

            if (_queuesConfiguration == null
                || !_queuesConfiguration.GetChildren().Any())
                throw new VectorizationException("The queues configuration is empty.");

            foreach (var rs in _settings!)
            {
                var connectionString = _queuesConfiguration[rs.ConnectionConfigurationName];
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new VectorizationException($"The configuration setting [{rs.ConnectionConfigurationName}] was not found.");
                rs.ConnectionString = connectionString;
            }
        }

        private IRequestSourceService GetRequestSourceService(RequestSourceServiceSettings settings, VectorizationQueuing queuing) =>
            queuing switch
            {
                VectorizationQueuing.None => new MemoryRequestSourceService(
                                        settings,
                                        _loggerFactory!.CreateLogger<MemoryRequestSourceService>()),
                VectorizationQueuing.AzureStorageQueue => new StorageQueueRequestSourceService(
                                        settings,
                                        _loggerFactory!.CreateLogger<StorageQueueRequestSourceService>()),
                _ => throw new VectorizationException($"The vectorization queuing mechanism [{queuing}] is not supported."),
            };
    }
}
