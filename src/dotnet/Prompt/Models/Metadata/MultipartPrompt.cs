using FoundationaLLM.Common.Models.ResourceProvider;
using System.Text.Json.Serialization;


namespace FoundationaLLM.Prompt.Models.Metadata
{
    /// <summary>
    /// Multipart prompt composed of a prefix and a suffix.
    /// </summary>
    public class MultipartPrompt : PromptBase
    {
        /// <summary>
        /// The prompt prefix.
        /// </summary>
        [JsonPropertyName("prefix")]
        public string? Prefix { get; set; }
        /// <summary>
        /// The prompt suffix.
        /// </summary>
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }
    }

}
