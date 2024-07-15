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

    /// <summary>
    /// Begins a completion operation.
    /// </summary>
    /// <param name="instanceId">The FoundationaLLM instance id.</param>
    /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
    /// <returns>Returns an <see cref="OperationState"/> object containing the OperationId and Status.</returns>
    Task<OperationState> StartCompletionOperation(string instanceId, CompletionRequest completionRequest);

    /// <summary>
    /// Gets a completion operation from the Gatekeeper service.
    /// </summary>
    /// <param name="instanceId">The FoundationaLLM instance id.</param>
    /// <param name="operationId">The ID of the operation to retrieve.</param>
    /// <returns>Returns a completion response</returns>
    Task<CompletionResponse> GetCompletionOperation(string instanceId, string operationId);
}
