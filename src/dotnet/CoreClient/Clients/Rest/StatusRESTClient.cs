using Azure.Core;
using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Settings;
using System.Text.Json;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    internal class StatusRESTClient(
        IHttpClientFactory httpClientFactory,
        TokenCredential credential) : CoreRESTClientBase(httpClientFactory, credential), IStatusRESTClient
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <inheritdoc/>
        public async Task<ServiceStatusInfo> GetServiceStatusAsync()
        {
            var coreClient = await GetCoreClientAsync();
            var response = await coreClient.GetAsync("status");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ServiceStatusInfo>(responseContent, _jsonSerializerOptions)!;
            }

            throw new Exception($"Failed to retrieve service status. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthStatusAsync()
        {
            var coreClient = await GetCoreClientAsync();
            var responseMessage = await coreClient.GetAsync("status/auth");

            if (responseMessage.IsSuccessStatusCode)
            {
                return "Authentication is successful.";
            }

            throw new Exception($"Failed to retrieve authentication status. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
