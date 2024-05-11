using FoundationaLLM.Gateway.Models;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// The result of a text embedding request.
    /// </summary>
    public class TextEmbeddingResult : OperationResult
    {
        /// <summary>
        /// The list of <see cref="TextChunk"/> objects containing the embeddings.
        /// </summary>
        [JsonPropertyName("text_chunks")]
        public IList<TextChunk> TextChunks { get; set; } = [];
    }
}
