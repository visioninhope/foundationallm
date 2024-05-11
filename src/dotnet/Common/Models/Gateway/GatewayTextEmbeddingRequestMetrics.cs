using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Gateway
{
    /// <summary>
    /// Provides metrics related to text embedding requests submitted by the FoundationaLLM Gateway.
    /// </summary>
    public class GatewayTextEmbeddingRequestMetrics : GatewayRequestMetrics
    {
        /// <summary>
        /// The number of text chunks in the current request.
        /// </summary>
        [JsonPropertyName("current_text_chunk_count")]
        public int CurrentTextChunkCount { get; set; }
    }
}
