using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// LLM orchestration request
    /// </summary>
    public class LLMCompletionRequest : CompletionRequestBase
    {
        /// <summary>
        /// The agent that will process the completion request.
        /// </summary>
        public required AgentBase Agent { get; set; }

        /// <summary>
        /// Collection of model settings to use with the orchestration request.
        /// </summary>
        [JsonPropertyName("settings")]
        public LLMOrchestrationSettings? Settings { get; set; }

    }
}
