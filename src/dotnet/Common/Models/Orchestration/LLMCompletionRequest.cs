using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// The completion request sent by the Orchestration API to any of the downstream orchestrator APIs.
    /// </summary>
    public class LLMCompletionRequest : CompletionRequestBase
    {
        /// <summary>
        /// The agent that will process the completion request.
        /// </summary>
        public required AgentBase Agent { get; set; }

        /// <summary>
        /// Dictionary of objects (indexed by names) resulting from exploding object identifiers in the Orchestration API.
        /// <para>
        /// Can also contain objects that are not the direct result of exploding an object identifier.
        /// </para>
        /// <para>
        /// The dictionary supports the following keys:
        /// <list type="bullet">
        /// <item>
        /// /instances/{instanceId}/providers/FoundationaLLM.Prompt/prompts/{name}
        /// </item>
        /// <item>
        /// /instances/{instanceId}/providers/FoundationaLLM.AIModel/aiModels/{name}
        /// </item>
        /// <item>
        /// /instances/{instanceId}/providers/FoundationaLLM.Configuration/apiEndpointConfigurations/{name}
        /// </item>
        /// <item>
        /// /instances/{instanceId}/providers/FoundationaLLM.Vectorization/indexingProfiles/{name}
        /// </item>
        /// <item>
        /// /instances/{instanceId}/providers/FoundationaLLM.Vectorization/textEmbeddingProfiles/{name}
        /// </item>
        /// <item>
        /// AllAgents
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        [JsonPropertyName("settings")]
        public Dictionary<string, object> Objects { get; set; } = [];

    }
}
