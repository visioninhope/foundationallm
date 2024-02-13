using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides services to interact with the Azure Resource Manager (ARM) infrastructure.
    /// </summary>
    public interface IAzureResourceManagerService
    {
        /// <summary>
        /// Creates a new Azure Event Grid namespace topic subscription.
        /// </summary>
        /// <param name="namespaceResourceId">The Azure resource identifier of the Azure Event Grid namespace.</param>
        /// <param name="topicName">The name of the topic for which the subscription should be created.</param>
        /// <param name="topicSubscriptionName">The name of the subscription to be created.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling the request to cancel the operation.</param>
        /// <returns>True if the subscription was successfully created, false otherwise.</returns>
        Task<bool> CreateEventGridNamespaceTopicSubscription(
            string namespaceResourceId,
            string topicName,
            string topicSubscriptionName,
            CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an Azure Event Grid namespace topic subscription.
        /// </summary>
        /// <param name="namespaceResourceId">The Azure resource identifier of the Azure Event Grid namespace.</param>
        /// <param name="topicName">The name of the topic for which the subscription should be deleted.</param>
        /// <param name="topicSubscriptionName">The name of the subscription to be deleted.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling the request to cancel the operation.</param>
        /// <returns></returns>
        Task DeleteEventGridNamespaceTopicSubscription(
            string namespaceResourceId,
            string topicName,
            string topicSubscriptionName,
            CancellationToken cancellationToken);
    }
}
