using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Common.Models.Orchestration;
using Newtonsoft.Json;

namespace FoundationaLLM.SemanticKernel.Core.Models
{
    /// <summary>
    /// The Knowledge Management Completion Request model.
    /// </summary>
    public class KnowledgeManagementCompletionRequest : LLMCompletionRequest
    {
        /// <summary>
        /// The Knowledge Management agent metadata.
        /// </summary>
        [JsonProperty("agent")]
        public required KnowledgeManagementAgent Agent { get; set; }
    }
}
