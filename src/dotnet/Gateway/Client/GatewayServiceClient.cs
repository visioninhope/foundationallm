using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Gateway.Client
{
    /// <summary>
    /// Provides methods to call the Gateway API service.
    /// </summary>
    public class GatewayServiceClient : IGatewayServiceClient
    {
        private readonly GatewayServiceSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GatewayServiceClient> _logger;

        /// <summary>
        /// Creates a new instance of the Gateway API service.
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> used to create the HTTP client.</param>
        /// <param name="options">The options object containing the <see cref="GatewayServiceSettings"/> object with the setting.</param>
        /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
        public GatewayServiceClient(
            IHttpClientFactory httpClientFactory,
            IOptions<GatewayServiceSettings> options,
            ILogger<GatewayServiceClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<bool> TryConsume(string modelId, int tokenCount)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"tryconsume?modelId={modelId}&tokenCount={tokenCount}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(responseContent);
            }

            return false;
        }

        public async Task<bool> AddModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod)
        {
            try
            {
                var client = GetHttpClient();

                var responseMessage = await client.GetAsync($"completions/AddModel?modelId={modelId}&requestRateLimit={requestRateLimit}&requestRateRenewalPeriod={requestRateRenewalPeriod}&tokenRateLimit={tokenRateLimit}&tokenRateRenewalPeriod={tokenRateRenewalPeriod}");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<bool>(responseContent);
                    return response!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding gateway model.");
                throw;
            }

            return false;
        }

        public async Task<CompletionResponse> GetCompletionOperationResult(string operationId)
        {
            CompletionResponse fallback = default;

            var client = GetHttpClient();
            var response = await client.GetAsync($"completions?operationId={operationId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var embeddingResult = JsonSerializer.Deserialize<CompletionResponse>(responseContent);

                return embeddingResult ?? fallback;
            }

            return fallback;
        }

        public async Task<TextEmbeddingResult> GetEmbeddingOperationResult(string operationId)
        {
            var fallback = new TextEmbeddingResult
            {
                InProgress = false,
                OperationId = null
            };

            var client = GetHttpClient();
            var response = await client.GetAsync($"embeddings?operationId={operationId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var embeddingResult = JsonSerializer.Deserialize<TextEmbeddingResult>(responseContent);

                return embeddingResult ?? fallback;
            }

            return fallback;
        }

        public async Task<CompletionResponse> StartCompletionOperation(CompletionRequest completionRequest)
        {
            CompletionResponse fallback = default;

            var client = GetHttpClient();
            var serializedRequest = JsonSerializer.Serialize(completionRequest);
            var response = await client.PostAsync("completions",
                new StringContent(
                    serializedRequest,
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var embeddingResult = JsonSerializer.Deserialize<CompletionResponse>(responseContent);

                return embeddingResult ?? fallback;
            }

            return fallback;
        }

        public async Task<TextEmbeddingResult> StartEmbeddingOperation(TextEmbeddingRequest embeddingRequest)
        {
            var fallback = new TextEmbeddingResult
            {
                InProgress = false,
                OperationId = null
            };

            var client = GetHttpClient();
            var serializedRequest = JsonSerializer.Serialize(embeddingRequest);
            var response = await client.PostAsync("embeddings",
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

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_settings.APIUrl);
            httpClient.DefaultRequestHeaders.Add(HttpHeaders.APIKey, _settings.APIKey);

            return httpClient;
        }
    }
}
