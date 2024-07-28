using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Service that provides a common interface for creating <see cref="HttpClient"/>
    /// instances from <see cref="IHttpClientFactory"/>. and ensures that all
    /// necessary headers are added to the request.
    /// </summary>
    public interface IHttpClientFactoryService
    {
        /// <summary>
        /// Creates a <see cref="HttpClient"/> instance from the provided client name.
        /// The client name must be registered in the <see cref="IHttpClientFactory"/> configuration.
        /// </summary>
        /// <param name="clientName">The named <see cref="HttpClient"/> client configuration.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <returns></returns>
        Task<HttpClient> CreateClient(string clientName, UnifiedUserIdentity? userIdentity);

        /// <summary>
        /// Creates a <see cref="HttpClient"/> instance from the provided endpoint configuration.
        /// </summary>
        /// <param name="endpointConfiguration">The endpoint configuration.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <returns></returns>
        Task<HttpClient> CreateClient(APIEndpointConfiguration endpointConfiguration, UnifiedUserIdentity? userIdentity);

        /// <summary>
        /// Creates a new unregistered <see cref="HttpClient"/> instance with a timeout.
        /// </summary>
        /// <param name="timeout">The timeout for the <see cref="HttpClient"/>.
        /// If not specified, the default timeout in seconds is applied.
        /// For an infinite waiting period, use <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <returns></returns>
        HttpClient CreateUnregisteredClient(TimeSpan? timeout = null);
    }
}
