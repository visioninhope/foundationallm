using FoundationaLLM.Client.Core.Clients.Rest;
using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace FoundationaLLM.Client.Core
{
    /// <inheritdoc/>
    public class CoreRESTClient : ICoreRESTClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRESTClient"/> class with
        /// the provided <see cref="IHttpClientFactory"/>. This constructor is used
        /// for dependency injection.
        /// </summary>
        /// <param name="httpClientFactory">An <see cref="IHttpClientFactory"/>
        /// configured with a named instance for the CoreAPI (<see cref="HttpClients.CoreAPI"/>).</param>
        public CoreRESTClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            InitializeClients(httpClientFactory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRESTClient"/> class and
        /// configures <see cref="IHttpClientFactory"/> with a named instance for the
        /// CoreAPI (<see cref="HttpClients.CoreAPI"/>) based on the passed in URL
        /// and optional timespan.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        public CoreRESTClient(string url, TimeSpan? timeout = null)
        {
            var services = new ServiceCollection();

            services.AddHttpClient(HttpClients.CoreAPI, client =>
            {
                client.BaseAddress = new Uri(url);
                if (timeout.HasValue)
                {
                    client.Timeout = timeout.Value;
                }
            });

            var serviceProvider = services.BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            InitializeClients(_httpClientFactory);
        }

        /// <inheritdoc/>
        public ISessionRESTClient Sessions { get; private set; }
        /// <inheritdoc/>
        public IAttachmentRESTClient Attachments { get; private set; }
        /// <inheritdoc/>
        public IBrandingRESTClient Branding { get; private set; }
        /// <inheritdoc/>
        public IOrchestrationRESTClient Orchestration { get; private set; }
        /// <inheritdoc/>
        public IStatusRESTClient Status { get; private set; }
        /// <inheritdoc/>
        public IUserProfileRESTClient UserProfiles { get; private set; }

        private void InitializeClients(IHttpClientFactory httpClientFactory)
        {
            Sessions = new SessionRESTClient(httpClientFactory);
            Attachments = new AttachmentRESTClient(httpClientFactory);
            Branding = new BrandingRESTClient(httpClientFactory);
            Orchestration = new OrchestrationRESTClient(httpClientFactory);
            Status = new StatusRESTClient(httpClientFactory);
            UserProfiles = new UserProfileRESTClient(httpClientFactory);
        }
    }
}
