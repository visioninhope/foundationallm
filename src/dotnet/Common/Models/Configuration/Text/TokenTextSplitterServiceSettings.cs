using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Configuration.Text
{
    /// <summary>
    /// Provides configuration settings that control token-based text splitting.
    /// </summary>
    /// <param name="EncoderName">The name of the encoder used for tokenization.</param>
    /// <param name="ChunkSizeTokens">The target size in tokens for the resulting text chunks.</param>
    /// <param name="OverlapSizeTokens">Teh target size in tokens for the overlapping parts of the adjacent text chunks.</param>
    public record TokenTextSplitterServiceSettings(
        string EncoderName,
        int ChunkSizeTokens,
        int OverlapSizeTokens)
    {
    }
}
