using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Core.Examples.Interfaces;

/// <summary>
/// Provides methods to manage calls to the Core API's sessions endpoints.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Creates and renames a session.
    /// </summary>
    /// <returns>Returns the new Session ID.</returns>
    /// <exception cref="FoundationaLLMException"></exception>
    Task<string> CreateSessionAsync();

    /// <summary>
    /// Sends a user prompt to the specified agent.
    /// </summary>
    /// <param name="orchestrationRequest"></param>
    /// <returns>Returns a completion response.</returns>
    /// <exception cref="FoundationaLLMException"></exception>
    Task<Completion> SendCompletionRequestAsync(OrchestrationRequest orchestrationRequest);

    /// <summary>
    /// Deletes a chat session.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task DeleteSessionAsync(string sessionId);
}