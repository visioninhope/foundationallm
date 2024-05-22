using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Provides details about an indexing profile.
    /// </summary>
    public class IndexingProfile050 : VectorizationProfileBase050
    {
        /// <summary>
        /// The type of the indexer.
        /// </summary>
        [JsonPropertyName("indexer")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required IndexerType050 Indexer { get; set; }
    }
}
