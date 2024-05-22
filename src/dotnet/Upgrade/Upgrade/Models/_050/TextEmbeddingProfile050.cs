using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Provides details about a text embedding profile.
    /// </summary>
    public class TextEmbeddingProfile050 : VectorizationProfileBase050
    {
        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonPropertyName("text_embedding")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextEmbeddingType050 TextEmbedding { get; set; }
    }
}
