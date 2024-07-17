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
    Task<ServiceStatusInfo> GetStatus();

    /// <summary>
    /// Retrieve a completion from the configured orchestration service.
    /// </summary>
    Task<ClientCompletionResponse> GetCompletion(ClientCompletionRequest completionRequest);
}
