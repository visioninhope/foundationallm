using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Core.Examples.Interfaces;

/// <summary>
/// Provides methods to manage calls to the Core API's orchestration endpoints.
/// </summary>
public interface IOrchestrationManager
{
    /// <summary>
    /// Sends a user prompt to the specified agent.
    /// </summary>
    /// <param name="completionRequest"></param>
    /// <returns></returns>
    Task<Completion> SendCompletionRequestAsync(CompletionRequest completionRequest);
}