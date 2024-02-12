using FoundationaLLM.Common.Models.Metadata;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Metadata
{
    /// <summary>
    /// Agent metadata model.
    /// </summary>
    public class Agent: MetadataBase
    {
        /// <summary>
        /// The prompt prefix to assign the agent.
        /// </summary>
        [JsonPropertyName("prompt_prefix")]
        public string? PromptPrefix { get; set; }

        /// <summary>
        /// The prompt suffix to assign the agent.
        /// </summary>
        [JsonPropertyName("prompt_suffix")]
        public string? PromptSuffix { get; set; }
    }
}
