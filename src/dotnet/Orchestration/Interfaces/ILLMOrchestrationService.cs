using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Orchestration.Core.Interfaces
{
    /// <summary>
    /// LLM Orchestration Service interface
    /// </summary>
    public interface ILLMOrchestrationService
    {
        /// <summary>
        /// The name of the LLM orchestration service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the status of the orchestration service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <returns></returns>
        Task<ServiceStatusInfo> GetStatus(string instanceId);

        /// <summary>
        /// Method for retrieving a completion from the orchestration service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="request">Hub populated request object containing agent, prompt, language model, and data source information</param>
        /// <returns></returns>
        Task<LLMCompletionResponse> GetCompletion(string instanceId, LLMCompletionRequest request);
    }
}
