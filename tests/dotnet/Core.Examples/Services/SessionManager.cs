using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Exceptions;
using FoundationaLLM.Core.Examples.Interfaces;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class SessionManager(IHttpClientManager httpClientManager) : ISessionManager
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <inheritdoc/>
        public async Task<string> CreateSessionAsync()
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.CoreAPI);
            var responseSession = await coreClient.PostAsync("sessions", null);

            if (responseSession.IsSuccessStatusCode)
            {
                var responseContent = await responseSession.Content.ReadAsStringAsync();
                var sessionResponse = JsonSerializer.Deserialize<Session>(responseContent, _jsonSerializerOptions);
                var sessionId = string.Empty;
                if (sessionResponse?.SessionId != null)
                {
                    sessionId = sessionResponse.SessionId;
                }

                var sessionName = "Test: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await coreClient.PostAsync($"sessions/{sessionId}/rename?newChatSessionName={UrlEncoder.Default.Encode(sessionName)}", null);

                return sessionId;
            }
            
            throw new FoundationaLLMException($"Failed to create a new chat session. Status code: {responseSession.StatusCode}. Reason: {responseSession.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<Completion> SendCompletionRequestAsync(OrchestrationRequest orchestrationRequest)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.CoreAPI);
            var serializedRequest = JsonSerializer.Serialize(orchestrationRequest, _jsonSerializerOptions);

            var sessionUrl = $"sessions/{orchestrationRequest.SessionId}/completion"; // Session-based - message history and data is retained in Cosmos DB. Must create a session if it does not exist.
            var responseMessage = await coreClient.PostAsync(sessionUrl,
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

        /// <inheritdoc/>
        public async Task DeleteSessionAsync(string sessionId)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.CoreAPI);
            await coreClient.DeleteAsync($"sessions/{sessionId}");
        }
    }
}
