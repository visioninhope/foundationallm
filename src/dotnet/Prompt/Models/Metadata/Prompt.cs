using FoundationaLLM.Common.Models.ResourceProvider;
using System.Text.Json.Serialization;


namespace FoundationaLLM.Prompt.Models.Metadata
{
    /// <summary>
    /// Prompt metadata model.
    /// </summary>
    public class Prompt : ResourceBase
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
