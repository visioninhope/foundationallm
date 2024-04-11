using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Interfaces;

namespace FoundationaLLM.Vectorization.Services.Text
{
    /// <summary>
    /// Generates text embeddings by routing requests through the FoundationaLLM Gateway API.
    /// </summary>
    /// <param name="gatewayService">The <see cref="IGatewayService"/> used to call the Gateway API.</param>
    public class GatewayTextEmbeddingService(
        IGatewayService gatewayService) : ITextEmbeddingService
    {
        private readonly IGatewayService _gatewayService = gatewayService;

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(IList<TextChunk> textChunks, string modelName) =>
            await _gatewayService.StartEmbeddingOperation(new TextEmbeddingRequest
            {
                EmbeddingModelName = modelName,
                TextChunks = textChunks
            });

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(string operationId) =>
            await _gatewayService.GetEmbeddingOperationResult(operationId);
    }
}
