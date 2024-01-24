using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Types of text splitters available for text partitioning.
    /// </summary>
    public enum TextSplitterType
    {
        /// <summary>
        /// Text splitter that uses token counts to partition text.
        /// </summary>
        TokenTextSplitter
    }
}
