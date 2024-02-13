using FoundationaLLM.Common.Models.Events;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides services to interact with an eventing engine.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Starts the event service, allowing it to initialize.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the event service, allowing it to cleanup.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task StopAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Executes the event service until cancellation is signaled.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds an event set event delgate to the list of event handlers for a specified event set namespace.
        /// </summary>
        /// <param name="eventNamespace">The name of the event namespace.</param>
        /// <param name="eventHandler">The function to be invoked during event handling.</param>
        void SubscribeToEventSetEvent(string eventNamespace, EventSetEventDelegate eventHandler);

        /// <summary>
        /// Removes an event set event delegate from the list of event handlers for a specified event set namespace.
        /// </summary>
        /// <param name="eventNamespace">The name of the event namespace.</param>
        /// <param name="eventHandler">The function to be invoked during event handling.</param>
        void UnsubscribeFromEventSetEvent(string eventNamespace, EventSetEventDelegate eventHandler);
    }
}
