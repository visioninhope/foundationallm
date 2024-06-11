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
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Client.Core
{
    /// <summary>
    /// Provides high-level methods to interact with the Core API.
    /// </summary>
    public class CoreClient(ICoreRESTClient coreRestClient) : ICoreClient
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
        public async Task<Completion> SendCompletionWithSessionAsync(string? sessionId, string? sessionName,
            string userPrompt, string agentName, string token)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                sessionId = await CreateChatSessionAsync(sessionName, token);
            }

            var orchestrationRequest = new OrchestrationRequest
            {
                AgentName = agentName,
                SessionId = sessionId,
                UserPrompt = userPrompt
            };
            var completion = await coreRestClient.Sessions.SendSessionCompletionRequestAsync(orchestrationRequest, token);
            return completion;
        }

        /// <inheritdoc/>
        public async Task<Completion> SendSessionlessCompletionAsync(string userPrompt, string agentName, string token)
        {
            var completionRequest = new CompletionRequest
            {
                UserPrompt = userPrompt
            };
            var completion = await coreRestClient.Orchestration.SendOrchestrationCompletionRequestAsync(completionRequest, token);
            return completion;
        }

        /// <inheritdoc/>
        public async Task<Completion> AttachFileAndAskQuestionAsync(Stream fileStream, string fileName, string contentType,
            string agentName, string question, bool useSession, string? sessionId, string? sessionName, string token)
        {
            var objectId = await coreRestClient.Attachments.UploadAttachmentAsync(fileStream, fileName, contentType, token);

            if (useSession)
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    sessionId = await CreateChatSessionAsync(sessionName, token);
                }

                var orchestrationRequest = new OrchestrationRequest
                {
                    AgentName = agentName,
                    SessionId = sessionId,
                    UserPrompt = question,
                    Attachments = [objectId]
                };
                var sessionCompletion = await coreRestClient.Sessions.SendSessionCompletionRequestAsync(orchestrationRequest, token);

                return sessionCompletion;
            }

            // Use the orchestrated completion request to ask a question about the file.
            var completionRequest = new CompletionRequest
            {
                AgentName = agentName,
                UserPrompt = question,
                Attachments = [objectId]
            };
            var completion = await coreRestClient.Orchestration.SendOrchestrationCompletionRequestAsync(completionRequest, token);

            return completion;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResourceProviderGetResult<AgentBase>>> GetAgentsAsync(string token)
        {
            var agents = await coreRestClient.Orchestration.GetAgentsAsync(token);
            return agents;
        }

        /// <inheritdoc/>
        public async Task DeleteSessionAsync(string sessionId, string token) => await coreRestClient.Sessions.DeleteSessionAsync(sessionId, token);
    }
}
