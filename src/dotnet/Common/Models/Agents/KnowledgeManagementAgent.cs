using FoundationaLLM.Agent.Constants;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Agents
{
    /// <summary>
    /// The Knowledge Management agent metadata model.
    /// </summary>
    public class KnowledgeManagementAgent : AgentBase
    {
        /// <summary>
        /// The vectorization indexing profile resource path.
        /// </summary>
        [JsonPropertyName("indexing_profile")]
        public string? IndexingProfile { get; set; }
        /// <summary>
        /// The vectorization embedding profile resource path.
        /// </summary>
        [JsonPropertyName("embedding_profile")]
        public string? EmbeddingProfile { get; set; }

        /// <summary>
        /// Set default property values.
        /// </summary>
        public KnowledgeManagementAgent() =>
            Type = AgentTypes.KnowledgeManagement;
    }
}
