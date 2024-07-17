using FoundationaLLM.Common.Models.ResourceProviders.AIModel;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Shared settings for an orchestration request
    /// </summary>
    public class OrchestrationSettingsBase
    {
        /// <summary>
        /// The agent's LLM orchestrator type.
        /// </summary>
        [JsonPropertyName("orchestrator")]
        public string? Orchestrator { get; set; }

        /// <summary>
        /// AzureAICompletionParameters to override the behavior of the agent.
        /// </summary>
        [JsonPropertyName("agent_parameters")]
        public Dictionary<string, object>? AgentParameters { get; set; }
    }
}
