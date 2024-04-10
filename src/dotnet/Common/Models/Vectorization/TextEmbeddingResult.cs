namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// The result of a text embedding request.
    /// </summary>
    public class TextEmbeddingResult
    {
        /// <summary>
        /// Indicates whether the text embedding operation is still in progress.
        /// When true, the <see cref="OperationId"/> property contains an operation identifier.
        /// </summary>
        public bool InProgress { get; set; }

        /// <summary>
        /// Optional operation identifier that can be used to retrieve the final result.
        /// </summary>
        public string? OperationId { get; set; }

        /// <summary>
        /// The list of <see cref="TextChunk"/> objects containing the embeddings.
        /// </summary>
        public IList<TextChunk> TextChunks { get; set; } = [];

        /// <summary>
        /// The number of tokens used during the embedding operation.
        /// </summary>
        public int TokenCount { get; set; }
    }
}
