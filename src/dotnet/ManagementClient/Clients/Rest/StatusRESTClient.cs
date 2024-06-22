using Azure.Core;
using FoundationaLLM.Client.Management.Interfaces;

namespace FoundationaLLM.Client.Management.Clients.Rest
{
    internal class StatusRESTClient(
        IHttpClientFactory httpClientFactory,
        TokenCredential credential) : ManagementRESTClientBase(httpClientFactory, credential), IStatusRESTClient
    {
        /// <inheritdoc/>
        public async Task<string> GetServiceStatusAsync()
        {
            var coreClient = await GetManagementClientAsync();
            var responseMessage = await coreClient.GetAsync("status");

            if (responseMessage.IsSuccessStatusCode)
            {
                return "Service is up and running.";
            }

            throw new Exception($"Failed to retrieve service status. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthStatusAsync()
        {
            var coreClient = await GetManagementClientAsync();
            var responseMessage = await coreClient.GetAsync("status/auth");

            if (responseMessage.IsSuccessStatusCode)
            {
                return "Authentication is successful.";
            }

            throw new Exception($"Failed to retrieve authentication status. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
