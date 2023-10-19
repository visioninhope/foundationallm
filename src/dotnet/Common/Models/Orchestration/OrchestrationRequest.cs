using Newtonsoft.Json;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Base class for orchestration request objects.
    /// </summary>
    public class OrchestrationRequest
    {
        /// <summary>
        /// Represent the input or user prompt.
        /// </summary>
        [JsonProperty("user_prompt")]
        public string UserPrompt { get; set; }

        /// <summary>
        /// used to keep track of a single request through the system
        /// </summary>
        [JsonProperty("correlation_id")]
        public string CorrelationId { get; set; }
    }
}
