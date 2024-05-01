using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class HttpClientManager(
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<HttpClientOptions> httpClientOptions) : IHttpClientManager
    {
        /// <inheritdoc/>
        public async Task<HttpClient> GetHttpClientAsync(string apiType)
        {
            var httpClient = httpClientFactory.CreateClient(apiType);
            var options = httpClientOptions.Get(apiType);

            var scope = options.Scope;
            // The scope needs to just be the base URI, not the full URI.
            if (scope != null)
            {
                scope = scope[..scope.LastIndexOf('/')];

                var credentials = DefaultAuthentication.GetAzureCredential();
                var tokenResult = await credentials.GetTokenAsync(
                    new([scope]),
                    default);

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenResult.Token);
            }

            return httpClient;
        }
    }
}
