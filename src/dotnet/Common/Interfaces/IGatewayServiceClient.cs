using FoundationaLLM.Common.Constants.OpenAI;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Defines the interface of the FoundationaLLM Gateway service client.
    /// </summary>
    public interface IGatewayServiceClient
    {
        /// <summary>
        /// Starts an embedding operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="embeddingRequest">The <see cref="TextEmbeddingRequest"/> object containing the details of the embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        Task<TextEmbeddingResult> StartEmbeddingOperation(string instanceId, TextEmbeddingRequest embeddingRequest);

        /// <summary>
        /// Retrieves the outcome of an embedding operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The unique identifier of the text embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        Task<TextEmbeddingResult> GetEmbeddingOperationResult(string instanceId, string operationId);

        /// <summary>
        /// Creates a new LLM-based agent capability.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="capabilityCategory">The category of the capability.</param>
        /// <param name="capabilityName">The name of the capability to be created.</param>
        /// <param name="parameters">A dictionary of parameter values used to create the capability.</param>
        /// <returns>A dictionary of output values.</returns>
        /// <remarks>
        /// The supported categories are:
        /// <list type="bullet">
        /// <item>
        /// OpenAI.Assistants (the names of the keys for the parameters and output dictionaries are defined in <see cref="OpenAIAgentCapabilityParameterNames"/>)
        /// </item>
        /// </list>
        /// </remarks>
        Task<Dictionary<string, object>> CreateAgentCapability(string instanceId, string capabilityCategory, string capabilityName, Dictionary<string, object>? parameters = null);
    }
}
