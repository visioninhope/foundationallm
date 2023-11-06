using Newtonsoft.Json;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.Metadata
{
    /// <summary>
    /// Agent metadata model.
    /// </summary>
    public class Agent: MetadataBase
    {
        /// <summary>
        /// The prompt template to assign the agent.
        /// </summary>
        [JsonProperty("prompt_template")]
        public string? PromptTemplate { get; set; }

        /// <summary>
        /// The prompt suffix to assign the agent.
        /// </summary>
        [JsonProperty("prompt_suffix")]
        public string? PromptSuffix { get; set; }
    }
}
