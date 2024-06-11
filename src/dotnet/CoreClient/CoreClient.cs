using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Client.Core
{
    public class CoreClient(ICoreRESTClient coreRestClient)
    {
        /// <inheritdoc/>
        public async Task<string> CreateChatSessionAsync(string? sessionName, string token)
        {
            var sessionId = await coreRestClient.Sessions.CreateSessionAsync(token);
            if (!string.IsNullOrWhiteSpace(sessionName))
            {
                await coreRestClient.Sessions.RenameChatSession(sessionId, sessionName, token);
            }

            return sessionId;
        }

        /// <inheritdoc/>
        public async Task<string> RenameChatSession(string sessionId, string sessionName, string token) => await coreRestClient.Sessions.RenameChatSession(sessionId, sessionName, token);

        /// <inheritdoc/>
        public async Task<Completion> SendSessionCompletionRequestAsync(OrchestrationRequest orchestrationRequest, string token) => await coreRestClient.Sessions.SendSessionCompletionRequestAsync(orchestrationRequest, token);

        /// <inheritdoc/>
        public async Task<CompletionPrompt> GetCompletionPromptAsync(string sessionId, string completionPromptId, string token) => await coreRestClient.Sessions.GetCompletionPromptAsync(sessionId, completionPromptId, token);

        /// <inheritdoc/>
        public async Task<IEnumerable<Message>> GetChatSessionMessagesAsync(string sessionId, string token) => await coreRestClient.Sessions.GetChatSessionMessagesAsync(sessionId, token);

        /// <inheritdoc/>
        public async Task DeleteSessionAsync(string sessionId, string token) => await coreRestClient.Sessions.DeleteSessionAsync(sessionId, token);
    }
}
