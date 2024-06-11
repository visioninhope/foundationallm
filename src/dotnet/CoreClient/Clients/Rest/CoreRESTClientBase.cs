using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Settings;
using System.Text.Json;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    internal class CoreRESTClientBase(IHttpClientFactory httpClientFactory)
    {
        /// <summary>
        /// Sets standard JSON serializer options.
        /// </summary>
        protected JsonSerializerOptions SerializerOptions { get; } =
            CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <summary>
        /// Returns a new HttpClient configured with an authentication header that uses the supplied token.
        /// </summary>
        /// <param name="token">The authentication token to send with the HTTP client requests.</param>
        /// <returns></returns>
        protected HttpClient GetCoreClient(string token)
        {
            var coreClient = httpClientFactory.CreateClient(HttpClients.CoreAPI);
            coreClient.SetBearerToken(token);
            return coreClient;
        }
    }
}
