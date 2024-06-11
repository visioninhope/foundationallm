using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Models.ResourceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FoundationaLLM.Client.Core.Interfaces;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's orchestration endpoints.
    /// </summary>
    public class OrchestrationRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), IOrchestrationRESTClient
    {
        /// <inheritdoc/>
        public async Task<Completion> SendOrchestrationCompletionRequestAsync(CompletionRequest completionRequest, string token)
        {
            var coreClient = GetCoreClient(token);
            var serializedRequest = JsonSerializer.Serialize(completionRequest, SerializerOptions);

            var responseMessage = await coreClient.PostAsync("orchestration/completion", // Session-less - no message history or data retention in Cosmos DB.
                new StringContent(
                    serializedRequest,
                    Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse =
                    JsonSerializer.Deserialize<Completion>(responseContent, SerializerOptions);
                return completionResponse ?? throw new InvalidOperationException("The returned completion response is invalid.");
            }

            throw new Exception($"Failed to send completion request. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResourceProviderGetResult<AgentBase>>> GetAgentsAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("orchestration/agents");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var agents = JsonSerializer.Deserialize<IEnumerable<ResourceProviderGetResult<AgentBase>>>(responseContent, SerializerOptions);
                return agents ?? throw new InvalidOperationException("The returned agents are invalid.");
            }

            throw new Exception($"Failed to retrieve agents. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
