using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Provides context associated with an embedding model deployment.
    /// </summary>
    public class EmbeddingModelDeploymentContext
    {
        private List<TextChunk> _inputTextChunks = [];

        /// <summary>
        /// The Azure OpenAI deployment for the model.
        /// </summary>
        public required AzureOpenAIAccountDeployment Deployment { get; set; }

        /// <summary>
        /// The <see cref="ITextEmbeddingService"/> exposing the capabilities to interact with the model.
        /// </summary>
        public required ITextEmbeddingService TextEmbeddingService { get; set; }

        public bool HasInput =>
            _inputTextChunks.Count > 0;

        public void ResetInput() =>
            _inputTextChunks = [];

        public bool TryAddInputTextChunk(TextChunk textChunk)
        {
            _inputTextChunks.Add(textChunk);
            return true;
        }

        public async Task<TextEmbeddingResult> GetEmbeddingsForInputTextChunks() =>
            await TextEmbeddingService.GetEmbeddingsAsync(_inputTextChunks);
    }
}
