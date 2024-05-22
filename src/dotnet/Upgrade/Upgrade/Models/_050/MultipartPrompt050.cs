using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    public class MultipartPrompt050 : Prompt050
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

        /// <summary>
        /// Set default property values.
        /// </summary>
        public MultipartPrompt050() =>
            Type = PromptTypes.Multipart;
    }
}
