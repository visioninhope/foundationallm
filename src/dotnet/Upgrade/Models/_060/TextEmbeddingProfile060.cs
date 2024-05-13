using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Provides details about a text embedding profile.
    /// </summary>
    public class TextEmbeddingProfile060 : VectorizationProfileBase060
    {
        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonPropertyName("text_embedding")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextEmbeddingType060 TextEmbedding { get; set; }
    }
}
