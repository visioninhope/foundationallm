using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Client.Core.Interfaces
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's orchestration endpoints.
    /// </summary>
    public interface IOrchestrationRESTClient
    {
        /// <summary>
        /// Performs a sessionless request to the Core API.
        /// </summary>
        /// <param name="completionRequest">The completion request data sent to the endpoint.</param>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<Completion> SendOrchestrationCompletionRequestAsync(CompletionRequest completionRequest, string token);

        /// <summary>
        /// Retrieves agents available to the user for orchestration and session-based requests.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<IEnumerable<ResourceProviderGetResult<AgentBase>>> GetAgentsAsync(string token);
    }
}
