using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Orchestration.Core.Interfaces;

/// <summary>
/// Interface for the Orchestration Service
/// </summary>
public interface IOrchestrationService
{
    /// <summary>
    /// Get the aggredated status of all orchestration services.
    /// </summary>
    /// <param name="instanceId">The FoundationaLLM instance id.</param>
    /// <returns>The status of the orchestration service.</returns>
    Task<ServiceStatusInfo> GetStatus(string instanceId);

    /// <summary>
    /// Retrieve a completion from the configured orchestration service.
    /// </summary>
    /// <param name="instanceId">The FoundationaLLM instance id.</param>
    /// <param name="completionRequest">The completion request.</param>
    /// <returns>The completion response.</returns>
    Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest);
}
