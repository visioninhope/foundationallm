using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Exceptions;
using FoundationaLLM.Gateway.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Gateway.Client
{
    /// <summary>
    /// Provides methods to call the Gateway API service.
    /// </summary>
    public class GatewayServiceClient : IGatewayServiceClient
    {
        private readonly IHttpClientFactoryService _httpClientFactory;
        private readonly ILogger<GatewayServiceClient> _logger;

        /// <summary>
        /// Creates a new instance of the Gateway API service.
        /// </summary>
        /// <param name="httpClientFactory">An <see cref="IHttpClientFactoryService"/> used to build HTTP clients for the Gateway API.</param>
        /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
        public GatewayServiceClient(
            IHttpClientFactoryService httpClientFactory,
            ILogger<GatewayServiceClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingOperationResult(string instanceId, string operationId, UnifiedUserIdentity userIdentity)
        {
            var fallback = new TextEmbeddingResult
            {
                InProgress = false,
                OperationId = null
            };

            var gatewayAPIHttpClient = await GetGatewayAPIHttpClient(userIdentity);
            var response = await gatewayAPIHttpClient.GetAsync($"instances/{instanceId}/embeddings?operationId={operationId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var embeddingResult = JsonSerializer.Deserialize<TextEmbeddingResult>(responseContent);

                return embeddingResult ?? fallback;
            }

            return fallback;
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> StartEmbeddingOperation(string instanceId, TextEmbeddingRequest embeddingRequest, UnifiedUserIdentity userIdentity)
        {
            var fallback = new TextEmbeddingResult
            {
                InProgress = false,
                OperationId = null
            };

            var serializedRequest = JsonSerializer.Serialize(embeddingRequest);

            var gatewayAPIHttpClient = await GetGatewayAPIHttpClient(userIdentity);
            var response = await gatewayAPIHttpClient.PostAsync($"instances/{instanceId}/embeddings",
                new StringContent(
                    serializedRequest,
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var embeddingResult = JsonSerializer.Deserialize<TextEmbeddingResult>(responseContent);

                return embeddingResult ?? fallback;
            }

            return fallback;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> CreateAgentCapability(
            string instanceId,
            string capabilityCategory,
            string capabilityName,
            UnifiedUserIdentity userIdentity,
            Dictionary<string, object>? parameters = null)
        {
            var serializedRequest = JsonSerializer.Serialize(new AgentCapabilityRequest
            {
                CapabilityCategory = capabilityCategory,
                CapabilityName = capabilityName,
                Parameters = parameters ?? []
            });
            var gatewayAPIHttpClient = await GetGatewayAPIHttpClient(userIdentity);
            var response = await gatewayAPIHttpClient.PostAsync($"instances/{instanceId}/agentcapabilities",
                new StringContent(
                    serializedRequest,
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

                if (responseObject == null || responseObject.Count == 0)
                    throw new GatewayException("The Gateway API returned an invalid response.");

                return responseObject;
            }

            throw new GatewayException($"The Gateway API returned an error status code ({response.StatusCode}) while processing the agent capability request.");
        }

        private async Task<HttpClient> GetGatewayAPIHttpClient(UnifiedUserIdentity userIdentity) =>
            await _httpClientFactory.CreateClient(
                HttpClientNames.GatewayAPI,
                userIdentity);
    }
}
