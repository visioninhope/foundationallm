using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants.Authentication;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoundationaLLM.Common.Services
{
    /// <inheritdoc/>
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly Dictionary<string, IResourceProviderService> _resourceProviderServices;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Creates a new instance of the <see cref="HttpClientFactoryService"/> class.
        /// </summary>
        /// <param name="resourceProviderServices">A list of of <see cref="IResourceProviderService"/> resource providers hashed by resource provider name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> used to retrieve app settings from configuration.</param>
        /// <param name="httpClientFactory">A fully configured <see cref="IHttpClientFactory"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HttpClientFactoryService(
            IEnumerable<IResourceProviderService> resourceProviderServices,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _resourceProviderServices = resourceProviderServices.ToDictionary(rps => rps.Name);
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        /// <inheritdoc/>
        public async Task<HttpClient> CreateClient(string clientName, UnifiedUserIdentity? userIdentity)
        {
            if (!_resourceProviderServices.TryGetValue(ResourceProviderNames.FoundationaLLM_Configuration, out var configurationResourceProvider))
                throw new Exception($"The resource provider {ResourceProviderNames.FoundationaLLM_Configuration} was not loaded.");

            var endpointConfiguration = await configurationResourceProvider.GetResource<APIEndpointConfiguration>(
                $"/{ConfigurationResourceTypeNames.APIEndpointConfigurations}/{clientName}",
                userIdentity);

            if (endpointConfiguration == null)
                throw new Exception($"The resource provider {ResourceProviderNames.FoundationaLLM_Configuration} did not load the {clientName} endpoint settings.");

            return await CreateClient(endpointConfiguration, userIdentity);
        }

        public async Task<HttpClient> CreateClient(APIEndpointConfiguration endpointConfiguration, UnifiedUserIdentity? userIdentity)
        {
            var httpClient = _httpClientFactory.CreateClient(endpointConfiguration.Name);

            // Set the default timeout.
            httpClient.Timeout = TimeSpan.FromSeconds(endpointConfiguration.TimeoutSeconds);

            // Set the default URL.
            httpClient.BaseAddress = new Uri(endpointConfiguration.Url);

            // Override the default URL if an exception is provided.
            if (userIdentity != null)
            {
                var urlException = endpointConfiguration.UrlExceptions.Where(x => x.UserPrincipalName == userIdentity.UPN).SingleOrDefault();
                if (urlException != null)
                    httpClient.BaseAddress = new Uri(urlException.Url);

                // Add the user identity header.
                var serializedIdentity = JsonSerializer.Serialize(userIdentity);
                httpClient.DefaultRequestHeaders.Add(Constants.HttpHeaders.UserIdentity, serializedIdentity);
            }

            switch (endpointConfiguration.AuthenticationType)
            {
                case AuthenticationTypes.APIKey:
                    var apiKey = string.Empty;
                    var apiKeyHeaderName = string.Empty;
                    var apiKeyPrefix = string.Empty;

                    if (!endpointConfiguration.AuthenticationParameters.TryGetValue(
                        AuthenticationParametersKeys.APIKeyConfigurationName, out var apiKeyConfigurationNameObj))
                        throw new Exception($"The {AuthenticationParametersKeys.APIKeyConfigurationName} key is missing from the enpoint's authentication parameters dictionary.");

                    apiKey = _configuration.GetValue<string>(apiKeyConfigurationNameObj?.ToString()!)!;

                    if (!endpointConfiguration.AuthenticationParameters.TryGetValue(
                        AuthenticationParametersKeys.APIKeyHeaderName, out var apiKeyHeaderNameObj))
                        throw new Exception($"The {AuthenticationParametersKeys.APIKeyHeaderName} key is missing from the enpoint's authentication parameters dictionary.");

                    apiKeyHeaderName = apiKeyHeaderNameObj.ToString();

                    if (!endpointConfiguration.AuthenticationParameters.TryGetValue(
                        AuthenticationParametersKeys.APIKeyPrefix, out var apiKeyPrefixObj))
                        throw new Exception($"The {AuthenticationParametersKeys.APIKeyPrefix} key is missing from the enpoint's authentication parameters dictionary.");

                    apiKeyPrefix = apiKeyPrefixObj.ToString();

                    if (apiKeyHeaderName == HeaderNames.Authorization)
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            apiKeyPrefix ?? string.Empty, apiKey
                        );
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Add(
                            apiKeyHeaderName,
                            string.IsNullOrWhiteSpace(apiKeyPrefix)
                                ? apiKey
                                : $"{apiKeyPrefix} {apiKey}");
                    }

                    break;

                case AuthenticationTypes.AzureIdentity:
                    if (!endpointConfiguration.AuthenticationParameters.TryGetValue(
                        AuthenticationParametersKeys.Scope, out var scopeObj))
                        throw new Exception($"The {AuthenticationParametersKeys.Scope} key is missing from the enpoint's authentication parameters dictionary.");

                    var scope = scopeObj as string;

                    if (string.IsNullOrEmpty(scope))
                        throw new Exception($"The {AuthenticationParametersKeys.Scope} key is missing from the enpoint's authentication parameters dictionary.");

                    var credentials = DefaultAuthentication.AzureCredential;
                    var tokenResult = await credentials!.GetTokenAsync(
                        new([scope]),
                        default);

                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", tokenResult.Token);

                    break;

                default:
                    throw new Exception($"The authentication type {endpointConfiguration.AuthenticationType} is not supported.");
            }

            return httpClient;
        }

        /// <inheritdoc/>
        public HttpClient CreateUnregisteredClient(TimeSpan? timeout = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = timeout ?? _defaultTimeout;
            return httpClient;
        }
    }
}
