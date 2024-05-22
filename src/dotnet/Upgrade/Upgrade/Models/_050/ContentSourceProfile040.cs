using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Provides details about a content source.
    /// </summary>
    public class ContentSourceProfile050 : VectorizationProfileBase050
    {
        /// <summary>
        /// The type of the content source.
        /// </summary>
        [JsonPropertyName("content_source")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required ContentSourceType050 ContentSource { get; set; }
    }
}
