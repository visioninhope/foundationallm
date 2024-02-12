using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Metadata;
using FoundationaLLM.Common.Models.Orchestration;
using System.Text.Json.Serialization;
using CommonMetadata = FoundationaLLM.Common.Models.Orchestration.Metadata;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration
{
    /// <summary>
    /// Orchestration completion request.
    /// Contains the metadata needed by the orchestration services
    /// to build and execute completions.
    /// </summary>
    public class LegacyCompletionRequest : LLMCompletionRequest
    {
        /// <summary>
        /// Agent metadata
        /// </summary>
        [JsonPropertyName("agent")]
        public CommonMetadata.Agent? Agent { get; set; }

        /// <summary>
        /// Data source metadata
        /// </summary>
        [JsonPropertyName("data_sources")]
        public List<MetadataBase>? DataSourceMetadata { get; set; }

        /// <summary>
        /// Language model metadata.
        /// </summary>
        [JsonPropertyName("language_model")]
        public LanguageModel? LanguageModel { get; set; }

        /// <summary>
        /// Embedding model metadata.
        /// </summary>
        [JsonPropertyName("embedding_model")]
        public EmbeddingModel? EmbeddingModel { get; set; }

        /// <summary>
        /// Message history list
        /// </summary>
        [JsonPropertyName("message_history")]
        public List<MessageHistoryItem>? MessageHistory { get; set; }
    }
}
