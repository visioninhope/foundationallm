using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Client.Core.Interfaces;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    public class StatusRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), IStatusRESTClient
    {
        /// <inheritdoc/>
        public async Task<string> GetServiceStatusAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("status");

            if (responseMessage.IsSuccessStatusCode)
            {
                return "Service is up and running.";
            }

            throw new Exception($"Failed to retrieve service status. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthStatusAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("status/auth");

            if (responseMessage.IsSuccessStatusCode)
            {
                return "Authentication is successful.";
            }

            throw new Exception($"Failed to retrieve authentication status. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
