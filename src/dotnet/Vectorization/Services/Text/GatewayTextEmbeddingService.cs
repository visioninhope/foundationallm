using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Vectorization;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Vectorization.Services.Text
{
    /// <summary>
    /// Generates text embeddings by routing requests through the FoundationaLLM Gateway API.
    /// </summary>
    /// <param name="instanceOptions">The options providing the <see cref="InstanceSettings"/> with instance settings.</param>
    /// <param name="gatewayService">The <see cref="IGatewayServiceClient"/> used to call the Gateway API.</param>
    public class GatewayTextEmbeddingService(
        IOptions<InstanceSettings> instanceOptions) : ITextEmbeddingService
    {
        private readonly InstanceSettings _instanceSettings = instanceOptions.Value;
        //private readonly IGatewayServiceClient _gatewayService = gatewayService;

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(IList<TextChunk> textChunks, string modelName) =>
            throw new NotImplementedException();
            //await _gatewayService.StartEmbeddingOperation(_instanceSettings.Id, new TextEmbeddingRequest
            //{
            //    EmbeddingModelName = modelName,
            //    TextChunks = textChunks
            //});

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(string operationId) =>
            throw new NotImplementedException();
            //await _gatewayService.GetEmbeddingOperationResult(_instanceSettings.Id, operationId);
    }
}
