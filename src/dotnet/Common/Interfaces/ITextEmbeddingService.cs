using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="text">The text which needs to be embedded.</param>
        /// <returns>Response containing the vector embedding and the amount of tokens used.</returns>
        Task<(ReadOnlyMemory<float> embedding, int tokenCount)> GetEmbeddingAsync(string text);

        /// <summary>
        /// Creates the vector embeddings for a specified list of texts.
        /// </summary>
        /// <param name="texts">The list of texts which need to be embedded.</param>
        /// <returns>Response containing the list of vector embeddings and the amount of tokens used.</returns>
        Task<(IList<ReadOnlyMemory<float>> embeddings, int tokenCount)> GetEmbeddingsAsync(IList<string> texts);
    }
}
