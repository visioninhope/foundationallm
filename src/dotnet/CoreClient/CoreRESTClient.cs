using FoundationaLLM.Client.Core.Clients.Rest;
using FoundationaLLM.Client.Core.Interfaces;

namespace FoundationaLLM.Client.Core
{
    /// <inheritdoc/>
    public class CoreRESTClient(IHttpClientFactory httpClientFactory) : ICoreRESTClient
    {
        /// <inheritdoc/>
        public ISessionRESTClient Sessions { get; } = new SessionRESTClient(httpClientFactory);
        /// <inheritdoc/>
        public IAttachmentRESTClient Attachments { get; } = new AttachmentRESTClient(httpClientFactory);
        /// <inheritdoc/>
        public IBrandingRESTClient Branding { get; } = new BrandingRESTClient(httpClientFactory);
        /// <inheritdoc/>
        public IOrchestrationRESTClient Orchestration { get; } = new OrchestrationRESTClient(httpClientFactory);
        /// <inheritdoc/>
        public IStatusRESTClient Status { get; } = new StatusRESTClient(httpClientFactory);
        /// <inheritdoc/>
        public IUserProfileRESTClient UserProfiles { get; } = new UserProfileRESTClient(httpClientFactory);
    }
}
