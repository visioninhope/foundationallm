using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Gateway.Interfaces
{
    /// <summary>
    /// Defines the interface of a FoundationaLLM Gateway service.
    /// </summary>
    public interface IGatewayService
    {
        /// <summary>
        /// Starts the Gateway service, allowing it to initialize.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the Gateway service, allowing it to cleanup.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task StopAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Executes the Gateway service until cancellation is signaled.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> signaling cancellation.</param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);

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
    }
}
