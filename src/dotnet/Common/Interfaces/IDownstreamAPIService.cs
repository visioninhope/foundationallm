using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Interface for calling a downstream API.
    /// </summary>
    public interface IDownstreamAPIService
    {
        /// <summary>
        /// The name of the downstream API.
        /// </summary>
        public string APIName { get; }

        /// <summary>
        /// Gets a completion from the downstream API.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest);
    }
}
