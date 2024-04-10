namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// Represents a chunk of text produced by a text splitter.
    /// </summary>
    public class TextChunk
    {
        /// <summary>
        /// The position of the text chunk in the content it belongs to.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// The text content of the chunk.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The embedding of the text content.
        /// </summary>
        public Embedding? Embedding { get; set; }

        /// <summary>
        /// The size of the chunk in tokens.
        /// </summary>
        public int TokensCount { get; set; }
    }
}
