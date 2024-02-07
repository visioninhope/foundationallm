using FoundationaLLM.Agent.Models.Metadata;
using Newtonsoft.Json;

namespace FoundationaLLM.SemanticKernel.Core.Models
{
    /// <summary>
    /// The Knowledge Management Completion Request model.
    /// </summary>
    public class KnowledgeManagementCompletionRequest
    {
        /// <summary>
        /// Represent the input or user prompt.
        /// </summary>
        [JsonProperty("user_prompt")]
        public required string UserPrompt { get; set; }

        /// <summary>
        /// The Knowledge Management agent metadata.
        /// </summary>
        [JsonProperty("agent")]
        public required KnowledgeManagementAgent Agent { get; set; }
    }
}
