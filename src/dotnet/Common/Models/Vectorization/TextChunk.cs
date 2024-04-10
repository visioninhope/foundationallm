namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// Represents a chunk of text produced by a text splitter.
    /// </summary>
    public class TextChunk
    {
        /// <summary>
        /// The text content of the chunk.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The size of the chunk in tokens.
        /// </summary>
        public int TokensCount { get; set; }
    }
}
