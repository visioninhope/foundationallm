using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging;
using Azure.Messaging.EventGrid.Namespaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Events;
using FoundationaLLM.Common.Models.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;

namespace FoundationaLLM.Common.Services.Events
{
    /// <summary>
    /// Provides services to integrate with the Azure Event Grid eventing platform.
    /// </summary>
    public class AzureEventGridEventService : IEventService
    {
        private readonly ILogger<AzureEventGridEventService> _logger;
        private readonly AzureEventGridEventServiceSettings _settings;
        private readonly AzureEventGridEventServiceProfile _profile;
        private readonly EventGridClient? _eventGridClient;

        private readonly TimeSpan _eventProcessingHeartbeat = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Creates a new instance of the <see cref="AzureEventGridEventService"/> event service.
        /// </summary>
        /// <param name="settingsOptions">The options providing the settings for the service.</param>
        /// <param name="profileOptions">The options providing the profile for the service.</param>
        /// <param name="logger">The logger used for logging.</param>
        public AzureEventGridEventService(
            IOptions<AzureEventGridEventServiceSettings> settingsOptions,
            IOptions<AzureEventGridEventServiceProfile> profileOptions,
            ILogger<AzureEventGridEventService> logger)
        {
            _settings = settingsOptions.Value;
            _profile = profileOptions.Value;
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

                foreach (var topic in  _profile.Topics)
                foreach (var subscription in topic.Subscriptions)
                {
                    if (cancellationToken.IsCancellationRequested) break;

                    try
                    {
                        var response = await _eventGridClient.ReceiveCloudEventsAsync(
                            topic.Name,
                            subscription.Name,
                            cancellationToken: cancellationToken);

                        if (response != null
                            && response.Value.Value != null
                            && response.Value.Value.Count > 0)
                        await ProcessTopicSubscriptionEvents(
                            topic.Name,
                            subscription.Name,
                            subscription.EventTypeHandlers,
                            response.Value.Value);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occured while trying to retrieve events for topic {TopicName} and subscription {SubscriptionName}.",
                            topic.Name, subscription.Name);
                    }
                }

                await Task.Delay(_eventProcessingHeartbeat, cancellationToken);
            }
        }

        private async Task ProcessTopicSubscriptionEvents(
            string topicName,
            string subscriptionName,
            List<EventGridEventTypeProfile> eventTypeProfiles, IReadOnlyList<ReceiveDetails> eventDetails)
        {

            foreach (var eventTypeProfile in eventTypeProfiles)
            foreach (var eventHandler in eventTypeProfile.EventHandlers)
            {
                var events = eventDetails
                    .Select(ed => ed.Event)
                    .Where(e =>
                        e.Type == eventTypeProfile.EventType
                        && e.Source == eventHandler.Source
                        && !string.IsNullOrWhiteSpace(e.Subject)
                        && e.Subject.StartsWith(eventHandler.SubjectPrefix))
                    .ToList();

                if (events.Count > 0)

            }

            await AcknowledgeMessages(topicName, subscriptionName, eventDetails);
        }

        private async Task AcknowledgeMessages(
            string topicName,
            string subscriptionName,
            IReadOnlyList<ReceiveDetails> eventDetails)
        {
            var result = await _eventGridClient!.AcknowledgeCloudEventsAsync(
                topicName,
                subscriptionName,
                new AcknowledgeOptions(
                    eventDetails.Select(ed => ed.BrokerProperties.LockToken)));

            foreach (var ackFailure in result.Value.FailedLockTokens)
            {
                _logger.LogError("Failed to acknowledge Event Grid message. Lock token: {LockToken}. Error code: {ErrorCode}. Error description: {ErrorDescription}.",
                    ackFailure.LockToken, ackFailure.Error, ackFailure.ToString());
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
