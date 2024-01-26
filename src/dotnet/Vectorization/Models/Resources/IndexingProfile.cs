using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Provides details about an indexing profile.
    /// </summary>
    public class IndexingProfile
    {
        /// <summary>
        /// The name of the indexing profile.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The type of the indexer.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required IndexerType Indexer { get; set; }

        /// <summary>
        /// The settings used to configure the indexer.
        /// </summary>
        public Dictionary<string, string>? IndexerSettings { get; set; }
    }
}
