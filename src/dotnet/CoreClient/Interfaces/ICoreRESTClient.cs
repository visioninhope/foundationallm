using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Client.Core.Interfaces
{
    /// <summary>
    /// Low-level REST API client for making direct calls to the Core API.
    /// </summary>
    public interface ICoreRESTClient
    {
        /// <summary>
        /// Creates and renames a session.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns>Returns the new Session ID.</returns>
        Task<string> CreateSessionAsync(string token);

        /// <summary>
        /// Renames a chat session.
        /// </summary>
        /// <param name="sessionId">The chat session ID.</param>
        /// <param name="sessionName">The new session name.</param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<string> RenameChatSession(string sessionId, string sessionName, string token);

        /// <summary>
        /// Sends a user prompt to the specified agent within the specified session.
        /// </summary>
        /// <param name="orchestrationRequest"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns>Returns a completion response.</returns>
        /// <exception cref="FoundationaLLMException"></exception>
        Task<Completion> SendSessionCompletionRequestAsync(OrchestrationRequest orchestrationRequest, string token);

        /// <summary>
        /// Gets a completion prompt by session ID and completion prompt ID.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="completionPromptId"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<CompletionPrompt> GetCompletionPromptAsync(string sessionId, string completionPromptId, string token);

        /// <summary>
        /// Returns the chat messages related to an existing session.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<IEnumerable<Message>> GetChatSessionMessagesAsync(string sessionId, string token);

        /// <summary>
        /// Sends a user prompt to the specified agent. Also considered a "sessionless" request.
        /// </summary>
        /// <param name="completionRequest"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<Completion> SendOrchestrationCompletionRequestAsync(CompletionRequest completionRequest, string token);

        /// <summary>
        /// Deletes a chat session.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task DeleteSessionAsync(string sessionId, string token);
    }
}
