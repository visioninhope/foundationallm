using Azure;
using Azure.Identity;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.Namespaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Common.Services.Events
{
    /// <summary>
    /// Provides services to integrate with the Azure Event Grid eventing platform.
    /// </summary>
    public class AzureEventGridEventService : IEventService
    {
        private readonly ILogger<AzureEventGridEventService> _logger;
        private readonly AzureEventGridEventServiceSettings _settings;
        private readonly EventGridClient? _eventGridClient;

        private record TopicSubscriptionPath(
            string Topic,
            string Subscription);

        private readonly TopicSubscriptionPath[] _topicSubscriptions = [
                new TopicSubscriptionPath("storage", "storage-core")
            ];

        private readonly TimeSpan _eventProcessingHeartbeat = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Creates a new instance of the <see cref="AzureEventGridEventService"/> event service.
        /// </summary>
        /// <param name="options">The <see cref="IOptions{TOptions}"/> providing the settings for the service.</param>
        /// <param name="logger">The logger used for logging.</param>
        public AzureEventGridEventService(
            IOptions<AzureEventGridEventServiceSettings> options,
            ILogger<AzureEventGridEventService> logger)
        {
            _settings = options.Value;
            _logger = logger;

            try
            {
                _eventGridClient = GetClient();
                if (_eventGridClient == null)
                    _logger.LogCritical("The Azure Event Grid client could not be initialized. The Azure Event Grid event service will not listen for any events.");
                else
                    _logger.LogInformation("The Azure Event Grid event service was successfully initialized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The Azure Event Grid client could not be initialized. The Azure Event Grid event service will not listen for any events.");
            }
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_eventGridClient == null)
                return;

            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;

                foreach (var topicSubscription in _topicSubscriptions)
                {
                    if (cancellationToken.IsCancellationRequested) break;

                    try
                    {
                        var result = await _eventGridClient.ReceiveCloudEventsAsync(
                            topicSubscription.Topic,
                            topicSubscription.Subscription,
                            cancellationToken: cancellationToken);

                        foreach (var eventDetail in result.Value.Value)
                        {
                            _logger.LogInformation(eventDetail.Event.Data.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occured while trying to retrieve events for topic {TopicName} and subscription {SubscriptionName}.",
                            topicSubscription.Topic, topicSubscription.Subscription);
                    }
                }

                await Task.Delay(_eventProcessingHeartbeat, cancellationToken);
            }
        }

        #region Create Event Grid client

        private void ValidateEndpoint(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogCritical("The Azure Event Grid namespace endpoint is invalid.");
                throw new ConfigurationValueException("The Azure Event Grid namespace endpoint is invalid.");
            }
        }

        private void ValidateAPIKey(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogCritical("The Azure Event Grid API key is invalid.");
                throw new ConfigurationValueException("The Azure Event Grid API key is invalid.");
            }
        }

        private EventGridClient? GetClient() =>
            _settings.AuthenticationType switch
            {
                AzureEventGridAuthenticationTypes.AzureIdentity => GetClientFromIdentity(),
                AzureEventGridAuthenticationTypes.APIKey => GetClientFromAPIKey(),
                _ => throw new ConfigurationValueException($"The {_settings.AuthenticationType} authentication type is not supported by the Azure Event Grid events service.")
            };

        private EventGridClient? GetClientFromIdentity()
        {
            EventGridClient? client = default;
            try
            {
                ValidateEndpoint(_settings.Endpoint);
                client = new EventGridClient(new Uri(_settings.Endpoint!), new DefaultAzureCredential());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the Azure Event Grid client.");
            }

            return client;
        }

        private EventGridClient? GetClientFromAPIKey()
        {
            EventGridClient? client = null;
            try
            {
                ValidateEndpoint(_settings.Endpoint);
                ValidateAPIKey(_settings.APIKey);
                client = new EventGridClient(new Uri(_settings.Endpoint!), new AzureKeyCredential(_settings.APIKey!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the Azure Event Grid client.");
            }

            return client;
        }

        #endregion
    }
}
