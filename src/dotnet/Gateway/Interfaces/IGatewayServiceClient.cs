using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Gateway.Interfaces
{
    /// <summary>
    /// Defines the interface of the FoundationaLLM Gateway service client.
    /// </summary>
    public interface IGatewayServiceClient
    {
        /// <summary>
        /// Starts an embedding operation.
        /// </summary>
        /// <param name="embeddingRequest">The <see cref="TextEmbeddingRequest"/> object containing the details of the embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        Task<TextEmbeddingResult> StartEmbeddingOperation(TextEmbeddingRequest embeddingRequest);

        /// <summary>
        /// Retrieves the outcome of an embedding operation.
        /// </summary>
        /// <param name="operationId">The unique identifier of the text embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        Task<TextEmbeddingResult> GetEmbeddingOperationResult(string operationId);

        /// <summary>
        /// Creates a new LLM-based agent capability.
        /// </summary>
        /// <param name="category">The category of the capability.</param>
        /// <param name="name">The name of the capability to be created.</param>
        /// <param name="parameters">A dictionary of parameter values used to create the capability. The keys of the dictionary are the parameter names.</param>
        /// <returns>The string identifier of the newly created capability.</returns>
        /// <remarks>
        /// The supported categories are:
        /// <list type="bullet">
        /// <item>OpenAI.Assistant</item>
        /// </list>
        /// </remarks>
        Task<string> CreateAgentCapability(string category, string name, Dictionary<string, object>? parameters = null);
    }
}
