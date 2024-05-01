using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Exceptions;
using FoundationaLLM.Core.Examples.Interfaces;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class OrchestrationManager(IHttpClientManager httpClientManager) : IOrchestrationManager
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <inheritdoc/>
        public async Task<Completion> SendCompletionRequestAsync(CompletionRequest completionRequest)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.CoreAPI);
            var serializedRequest = JsonSerializer.Serialize(completionRequest, _jsonSerializerOptions);

            var responseMessage = await coreClient.PostAsync("orchestration/completion", // Session-less - no message history or data retention in Cosmos DB.
                new StringContent(
                    serializedRequest,
                    Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse =
                    JsonSerializer.Deserialize<Completion>(responseContent, _jsonSerializerOptions);
                return completionResponse ?? throw new InvalidOperationException("The returned completion response is invalid.");
            }

            throw new FoundationaLLMException($"Failed to send completion request. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
