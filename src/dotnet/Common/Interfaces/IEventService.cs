using Azure.Messaging;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides services to interact with an eventing engine.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Executes the event service until cancellation is signaled.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
