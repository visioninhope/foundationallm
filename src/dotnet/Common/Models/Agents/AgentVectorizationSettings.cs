using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Agents
{
    /// <summary>
    /// Vectorization settings related to a knowledge management agent.
    /// </summary>
    public class AgentVectorizationSettings
    {
        /// <summary>
        /// Determines if the agent uses a dedicated pipeline.
        /// </summary>
        public bool DedicatedPipeline { get; set; }
        /// <summary>
        /// The data source resource path.
        /// </summary>
        [JsonPropertyName("data_source_object_id")]
        public string? DataSourceObjectId { get; set; }

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
        /// The vectorization data pipeline object path.
        /// </summary>
        [JsonPropertyName("vectorization_data_pipeline_object_id")]
        public string? VectorizationDataPipelineObjectId { get; set; }
    }
}
