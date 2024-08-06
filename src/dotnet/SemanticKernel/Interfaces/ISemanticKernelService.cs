using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.SemanticKernel.Core.Interfaces
{
    /// <summary>
    /// Defines methods for processing requests targeting the Semantic Kernel agents.
    /// </summary>
    public interface ISemanticKernelService
    {
        /// <summary>
        /// Gets a completion using Semantic Kernel agents.
        /// </summary>
        /// <param name="request">The <see cref="LLMCompletionRequest"/> containing the details of the completion request.</param>
        /// <returns>A <see cref="LLMCompletionResponse"/> with the results of the completion.</returns>
        Task<LLMCompletionResponse> GetCompletion(LLMCompletionRequest request);

        /// <summary>
        /// Begins a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>Returns an <see cref="LongRunningOperation"/> object containing the OperationId and Status.</returns>
        Task<LongRunningOperation> StartCompletionOperation(string instanceId, LLMCompletionRequest completionRequest);

        /// <summary>
        /// Gets the status of a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The OperationId for which to retrieve the status.</param>
        /// <returns>Returns an <see cref="LongRunningOperation"/> object containing the OperationId and Status.</returns>
        Task<LongRunningOperation> GetCompletionOperationStatus(string instanceId, string operationId);

        /// <summary>
        /// Gets a completion operation from the Orchestration service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The ID of the operation to retrieve.</param>
        /// <returns>Returns a <see cref="LLMCompletionResponse" /> object.</returns>
        Task<LLMCompletionResponse> GetCompletionOperationResult(string instanceId, string operationId);
    }
}
