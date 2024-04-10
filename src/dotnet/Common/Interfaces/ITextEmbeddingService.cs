using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides text embedding capabilities.
    /// </summary>
    public interface ITextEmbeddingService
    {
        /// <summary>
        /// Creates the vector embedding for a specified text.
        /// </summary>
        /// <param name="textChunk">The text chunk which needs to be embedded.</param>
        /// <returns>Response containing the vector embedding and the amount of tokens used.</returns>
        Task<(Embedding Embedding, int TokenCount)> GetEmbeddingAsync(TextChunk textChunk);

        /// <summary>
        /// Creates the vector embeddings for a specified list of texts.
        /// </summary>
        /// <param name="textChunks">The list of text chunks which need to be embedded.</param>
        /// <returns>Response containing the list of vector embeddings and the amount of tokens used.</returns>
        Task<(IList<Embedding> Embeddings, int TokenCount)> GetEmbeddingsAsync(IList<TextChunk> textChunks);
    }
}
