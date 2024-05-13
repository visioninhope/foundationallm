using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Provides details about an indexing profile.
    /// </summary>
    public class IndexingProfile060 : VectorizationProfileBase060
    {
        /// <summary>
        /// The type of the indexer.
        /// </summary>
        [JsonPropertyName("indexer")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required IndexerType060 Indexer { get; set; }
    }
}
