using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Exceptions;
using FoundationaLLM.Core.Examples.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class VectorizationAPITestManager(
        IHttpClientManager httpClientManager,
        IOptions<InstanceSettings> instanceSettings) : IVectorizationAPITestManager
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        public async Task<VectorizationResult> CreateVectorizationRequest(VectorizationRequest vectorizationRequest)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.VectorizationAPI);
            coreClient.BaseAddress = new Uri("https://localhost:7047");
            var serializedRequest = JsonSerializer.Serialize(vectorizationRequest, _jsonSerializerOptions);

            var response = await coreClient.PostAsync($"VectorizationRequest",
                               new StringContent(serializedRequest, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var resources = JsonSerializer.Deserialize<VectorizationResult>(responseContent, _jsonSerializerOptions);
                return resources;
            }

            throw new FoundationaLLMException($"Failed to upsert resource. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        public async Task<string> CheckVectorizationRequest(VectorizationRequest vectorizationRequest)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.VectorizationAPI);
            coreClient.BaseAddress = new Uri("https://localhost:7047");
            var serializedRequest = JsonSerializer.Serialize(vectorizationRequest, _jsonSerializerOptions);

            var response = await coreClient.PostAsync($"requeststatus",
                               new StringContent(serializedRequest, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }

            throw new FoundationaLLMException($"Failed to upsert resource. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        public async Task DeleteVectorizationRequest(VectorizationRequest vectorizationRequest)
        {
            return;
        }
    }
}
