using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Gatekeeper.Core.Interfaces;

/// <summary>
/// Interface for the Gatekeeper service.
/// </summary>
public interface IGatekeeperService
{
    /// <summary>
    /// Gets a completion from the Gatekeeper service.
    /// </summary>
    /// <param name="instanceId">The FoundationaLLM instance id.</param>
    /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
    /// <returns>The completion response.</returns>
    Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest);
}
