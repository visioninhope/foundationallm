using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Client.Core.Interfaces
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's session endpoints.
    /// </summary>
    public interface ISessionRESTClient
    {
        /// <summary>
        /// Retrieves all chat sessions.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<IEnumerable<Session>> GetAllChatSessionsAsync(string token);

        /// <summary>
        /// Sets the rating for a message.
        /// </summary>
        /// <param name="sessionId">The chat session ID that contains the message to rate.</param>
        /// <param name="messageId">The ID of the message to rate.</param>
        /// <param name="rating">Set to true for a positive rating and false for a negative rating.</param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task RateMessageAsync(string sessionId, string messageId, bool rating, string token);

        /// <inheritdoc/>
        Task<string> SummarizeChatSessionNameAsync(string sessionId, string prompt, string token);

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
        /// Deletes a chat session.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task DeleteSessionAsync(string sessionId, string token);
    }
}
