using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._060
{
    public class MultipartPrompt060 : Prompt060
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
        public MultipartPrompt060() =>
            Type = PromptTypes.Multipart;
    }
}
