using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace FoundationaLLM.Client.Core
{
    /// <inheritdoc/>
    public class CoreRESTClient(IHttpClientFactory httpClientFactory) : ICoreRESTClient
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <inheritdoc/>
        public async Task<string> CreateSessionAsync(string token)
        {
            var coreClient = GetCoreClient(token);
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

            throw new Exception($"Failed to create a new chat session. Status code: {responseSession.StatusCode}. Reason: {responseSession.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<string> RenameChatSession(string sessionId, string sessionName, string token)
        {
            var coreClient = GetCoreClient(token);
            var response = await coreClient.PostAsync($"sessions/{sessionId}/rename?newChatSessionName={UrlEncoder.Default.Encode(sessionName)}", null);

            if (response.IsSuccessStatusCode)
            {
                return sessionName;
            }

            throw new Exception($"Failed to rename chat session. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<Completion> SendSessionCompletionRequestAsync(OrchestrationRequest orchestrationRequest, string token)
        {
            var coreClient = GetCoreClient(token);
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

            throw new Exception($"Failed to send completion request. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<CompletionPrompt> GetCompletionPromptAsync(string sessionId, string completionPromptId, string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync($"sessions/{sessionId}/completionprompts/{completionPromptId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionPrompt =
                    JsonSerializer.Deserialize<CompletionPrompt>(responseContent, _jsonSerializerOptions);
                return completionPrompt ?? throw new InvalidOperationException("The returned completion prompt is invalid.");
            }

            throw new Exception($"Failed to get completion prompt. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Message>> GetChatSessionMessagesAsync(string sessionId, string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync($"sessions/{sessionId}/messages");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var messages = JsonSerializer.Deserialize<IEnumerable<Message>>(responseContent, _jsonSerializerOptions);
                return messages ?? throw new InvalidOperationException("The returned messages are invalid.");
            }

            throw new Exception($"Failed to get chat session messages. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<Completion> SendOrchestrationCompletionRequestAsync(CompletionRequest completionRequest, string token)
        {
            var coreClient = GetCoreClient(token);
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

            throw new Exception($"Failed to send completion request. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task DeleteSessionAsync(string sessionId, string token)
        {
            var coreClient = GetCoreClient(token);
            await coreClient.DeleteAsync($"sessions/{sessionId}");
        }

        private HttpClient GetCoreClient(string token)
        {
            var coreClient = httpClientFactory.CreateClient(HttpClients.CoreAPI);
            coreClient.SetBearerToken(token);
            return coreClient;
        }
    }
}
