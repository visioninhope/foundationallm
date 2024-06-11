﻿using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Branding;
using System.Text.Json;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's Branding endpoints.
    /// </summary>
    internal class BrandingRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), IBrandingRESTClient
    {
        /// <inheritdoc/>
        public async Task<ClientBrandingConfiguration> GetBrandingAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("branding");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var branding = JsonSerializer.Deserialize<ClientBrandingConfiguration>(responseContent, SerializerOptions);
                return branding ?? throw new InvalidOperationException("The returned branding information is invalid.");
            }

            throw new Exception($"Failed to retrieve branding information. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
