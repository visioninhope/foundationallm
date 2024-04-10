namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// Models a request to embed a list of text chunks.
    /// </summary>
    public class TextEmbeddingRequest
    {
        /// <summary>
        /// The list of <see cref="TextChunk"/> objects containing the texts to embed.
        /// </summary>
        public IList<TextChunk> TestChunks { get; set; } = [];

        /// <summary>
        /// The name of the embedding model to use.
        /// If not specified, a default embedding model should be used.
        /// </summary>
        public string? EmbeddingModelName { get; set; }
    }
}
