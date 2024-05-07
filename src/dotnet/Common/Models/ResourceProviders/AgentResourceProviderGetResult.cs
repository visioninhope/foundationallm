using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders
{
    /// <summary>
    /// Represents the result of a GET Agent operation.
    /// </summary>
    public class AgentResourceProviderGetResult : ResourceProviderGetResult
    {
        /// <summary>
        /// Base agent metadata.
        /// </summary>
        [JsonPropertyName("agent")]
        public required AgentBase Agent { get; set; }
    }
}
