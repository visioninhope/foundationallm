using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Settings for an orchestration request.
    /// </summary>
    public class OrchestrationSettings
    {
        /// <summary>
        /// The name of the selected agent.
        /// </summary>
        [JsonPropertyName("agent_name")]
        public string AgentName { get; set; }
        /// <summary>
        /// Options to override the default behavior of the agent's language model.
        /// </summary>
        [JsonPropertyName("model_settings")]
        public Dictionary<string, object> ModelSettings { get; set; }
    }
}
