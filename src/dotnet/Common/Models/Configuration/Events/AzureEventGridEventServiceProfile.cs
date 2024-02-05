using FoundationaLLM.Common.Services.Events;

namespace FoundationaLLM.Common.Models.Configuration.Events
{
    /// <summary>
    /// The profile used to configure event handling in the <see cref="AzureEventGridEventService"/> event service.
    /// </summary>
    public class AzureEventGridEventServiceProfile
    {
        /// <summary>
        /// The list of <see cref="EventGridTopicProfile"/> topic profiles used to configure event handling for an Azure Event Grid namespace topic.
        /// </summary>
        public List<EventGridTopicProfile> Topics { get; set; } = [];
    }

    /// <summary>
    /// The profile used to configure event handling for an Azure Event Grid namespace topic.
    /// </summary>
    public class EventGridTopicProfile
    {
        /// <summary>
        /// The name of the Azure Event Grid namespace topic.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The list of <see cref="EventGridTopicSubscriptionProfile"/> topic subscription profiles used to configure event handling for an Azure Event Grid namespace topic subscription.
        /// </summary>
        public List<EventGridTopicSubscriptionProfile> Subscriptions { get; set; } = [];
    }

    /// <summary>
    /// The profile used to configure event handling for an Azure Event Grid namespace topic subscription.
    /// </summary>
    public class EventGridTopicSubscriptionProfile
    {
        /// <summary>
        /// The name of the Azure Event Grid namespace topic subscription.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The list of <see cref="EventGridEventTypeProfile"/> event handling profiles used to configure handling for event types.
        /// </summary>
        public List<EventGridEventTypeProfile> EventTypeHandlers { get; set; } = [];
    }

    /// <summary>
    /// The profile used to configure event handling for a specified Azure Event Grid event type.
    /// </summary>
    public class EventGridEventTypeProfile
    {
        /// <summary>
        /// The name of the Azure Event Grid event type.
        /// </summary>
        public required string EventType { get; set; }

        /// <summary>
        /// The list of <see cref="EventGridEventHandlerProfile"/> event handling profiles used to configure event handling for a specific subset of events of a specified event type.
        /// </summary>
        public List<EventGridEventHandlerProfile> EventHandlers { get; set; } = [];
    }

    /// <summary>
    /// The event handler profile used to configure event handling for a specific subset of events of a specified event type.
    /// </summary>
    public class EventGridEventHandlerProfile
    {
        /// <summary>
        /// The event source to which the handler applies.
        /// </summary>
        public required string Source { get; set; }

        /// <summary>
        /// The event subject prefix to which the handler applies.
        /// </summary>
        public required string SubjectPrefix { get; set; }

        /// <summary>
        /// The name of the event handler to which FoundationaLLM event subscribers can attach handlers.
        /// </summary>
        public required string EventHandlerName { get; set; }
    }
}
