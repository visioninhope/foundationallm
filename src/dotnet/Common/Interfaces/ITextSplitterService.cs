using FoundationaLLM.Common.Models.Vectorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Represents a text splitter.
    /// </summary>
    public interface ITextSplitterService
    {
        /// <summary>
        /// Splits plain text into multiple chunks.
        /// </summary>
        /// <param name="text">The plain text to split.</param>
        /// <returns>A list of <see cref="TextChunk"/> items containing the text chunks and their sizes in tokens.</returns>
        List<TextChunk> SplitPlainText(string text);
    }
}
