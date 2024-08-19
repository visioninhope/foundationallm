using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Defines methods to call the Gateway API service.
    /// </summary>
    public interface IGatewayServiceClient
    {
        /// <summary>
        /// Creates the artifacts required to provide a specified agent capability.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="capabilityCategory">The category of the agent capability.</param>
        /// <param name="capabilityName">The name of the newly created capability.</param>
        /// <param name="userIdentity">The <see cref="UnifiedUserIdentity"/> providing the identity of the calling user.</param>
        /// <param name="parameters">A dictionary of object values used to control the creation of the capability.</param>
        /// <returns>A dictionary of object values providing the details of the artifacts created as part of the agent capability.</returns>
        /// <remarks>
        /// The value of the <paramref name="capabilityCategory"/> parameter must be one of the values defined in the <see cref="AgentCapabilityCategoryNames"/> class.
        /// </remarks>
        Task<Dictionary<string, object>> CreateAgentCapability(string instanceId, string capabilityCategory, string capabilityName, UnifiedUserIdentity userIdentity, Dictionary<string, object>? parameters = null);

        /// <summary>
        /// Gets the result of an embedding operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The embedding operation id.</param>
        /// <param name="userIdentity">The <see cref="UnifiedUserIdentity"/> providing the identity of the calling user.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object holding the result of the embedding operation.</returns>
        Task<TextEmbeddingResult> GetEmbeddingOperationResult(string instanceId, string operationId, UnifiedUserIdentity userIdentity);

        /// <summary>
        /// Starts an embedding operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="embeddingRequest">A <see cref="TextEmbeddingRequest"/> providing the details of the embedding operation.</param>
        /// <param name="userIdentity">The <see cref="UnifiedUserIdentity"/> providing the identity of the calling user.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object holding the result of the embedding operation.</returns>
        Task<TextEmbeddingResult> StartEmbeddingOperation(string instanceId, TextEmbeddingRequest embeddingRequest, UnifiedUserIdentity userIdentity);
    }
}
