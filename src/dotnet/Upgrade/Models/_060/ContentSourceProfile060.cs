using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Provides details about a content source.
    /// </summary>
    public class ContentSourceProfile060 : VectorizationProfileBase060
    {
        /// <summary>
        /// The type of the content source.
        /// </summary>
        [JsonPropertyName("content_source")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required ContentSourceType060 ContentSource { get; set; }
    }
}
