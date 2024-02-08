using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Common.Models.Orchestration;
using System.Text.Json.Serialization;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration
{
    /// <summary>
    /// The Knowledge Management Completion Request model.
    /// </summary>
    public class KnowledgeManagementCompletionRequest : LLMOrchestrationCompletionRequest
    {
        /// <summary>
        /// The Knowledge Management agent metadata.
        /// </summary>
        [JsonPropertyName("agent")]
        public required KnowledgeManagementAgent Agent { get; set; }
    }
}
