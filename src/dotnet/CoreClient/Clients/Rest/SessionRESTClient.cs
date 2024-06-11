using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's session endpoints.
    /// </summary>
    public class SessionRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), ISessionRESTClient
    {
        /// <inheritdoc/>
        public async Task<string> CreateSessionAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseSession = await coreClient.PostAsync("sessions", null);

            if (responseSession.IsSuccessStatusCode)
            {
                var responseContent = await responseSession.Content.ReadAsStringAsync();
                var sessionResponse = JsonSerializer.Deserialize<Session>(responseContent, SerializerOptions);
                if (sessionResponse?.SessionId != null)
                {
                    return sessionResponse.SessionId;
                }
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
            var serializedRequest = JsonSerializer.Serialize(orchestrationRequest, SerializerOptions);

            var sessionUrl = $"sessions/{orchestrationRequest.SessionId}/completion"; // Session-based - message history and data is retained in Cosmos DB. Must create a session if it does not exist.
            var responseMessage = await coreClient.PostAsync(sessionUrl,
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
        public async Task<CompletionPrompt> GetCompletionPromptAsync(string sessionId, string completionPromptId, string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync($"sessions/{sessionId}/completionprompts/{completionPromptId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionPrompt =
                    JsonSerializer.Deserialize<CompletionPrompt>(responseContent, SerializerOptions);
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
                var messages = JsonSerializer.Deserialize<IEnumerable<Message>>(responseContent, SerializerOptions);
                return messages ?? throw new InvalidOperationException("The returned messages are invalid.");
            }

            throw new Exception($"Failed to get chat session messages. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Session>> GetAllChatSessionsAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("sessions");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var sessions = JsonSerializer.Deserialize<IEnumerable<Session>>(responseContent, SerializerOptions);
                return sessions ?? throw new InvalidOperationException("The returned sessions are invalid.");
            }

            throw new Exception($"Failed to retrieve chat sessions. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task RateMessageAsync(string sessionId, string messageId, bool rating, string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.PostAsync($"sessions/{sessionId}/message/{messageId}/rate?rating={rating}", null);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to rate message. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
            }
        }

        /// <inheritdoc/>
        public async Task<string> SummarizeChatSessionNameAsync(string sessionId, string prompt, string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.PostAsync($"sessions/{sessionId}/summarize-name", new StringContent(JsonSerializer.Serialize(prompt), Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completion = JsonSerializer.Deserialize<Completion>(responseContent, SerializerOptions);
                return completion?.Text ?? throw new InvalidOperationException("The returned summary is invalid.");
            }

            throw new Exception($"Failed to summarize chat session name. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task DeleteSessionAsync(string sessionId, string token)
        {
            var coreClient = GetCoreClient(token);
            await coreClient.DeleteAsync($"sessions/{sessionId}");
        }
    }
}
