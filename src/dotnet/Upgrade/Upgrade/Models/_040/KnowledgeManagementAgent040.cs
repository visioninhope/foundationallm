using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    /// <summary>
    /// The Knowledge Management agent metadata model.
    /// </summary>
    public class KnowledgeManagementAgent050 : Agent040
    {
        /// <summary>
        /// The vectorization content source profile.
        /// </summary>
        [JsonPropertyName("content_source_profile_object_id")]
        public string? ContentSourceProfileObjectId { get; set; }

        /// <summary>
        /// The vectorization indexing profile resource path.
        /// </summary>
        [JsonPropertyName("indexing_profile_object_id")]
        public string? IndexingProfileObjectId { get; set; }

        /// <summary>
        /// The vectorization text embedding profile resource path.
        /// </summary>
        [JsonPropertyName("text_embedding_profile_object_id")]
        public string? TextEmbeddingProfileObjectId { get; set; }

        /// <summary>
        /// The vectorization text partitioning profile resource path. 
        /// </summary>
        [JsonPropertyName("text_partitioning_profile_object_id")]
        public string? TextPartitioningProfileObjectId { get; set; }

        /// <summary>
        /// Set default property values.
        /// </summary>
        public KnowledgeManagementAgent050() =>
            Type = AgentTypes.KnowledgeManagement;
    }
}
