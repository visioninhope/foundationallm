using Newtonsoft.Json;

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
        [JsonProperty("prefix")]
        public string? Prefix { get; set; }
        /// <summary>
        /// The prompt suffix.
        /// </summary>
        [JsonProperty("suffix")]
        public string? Suffix { get; set; }
    }

}
