using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Basic properties for vectorization profiles.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(ContentSourceProfile050), "content-source-profile")]
    [JsonDerivedType(typeof(TextPartitioningProfile050), "text-partitioning-profile")]
    [JsonDerivedType(typeof(TextEmbeddingProfile050), "text-embedding-profile")]
    [JsonDerivedType(typeof(IndexingProfile050), "indexing-profile")]
    public class VectorizationProfileBase050 : ResourceBase050
    {
        /// <summary>
        /// The type of the vectorization profile.
        /// </summary>
        [JsonIgnore]
        public override string? Type { get; set; }

        /// <summary>
        /// The configuration associated with the vectorization profile.
        /// </summary>
        [JsonPropertyName("settings")]
        public Dictionary<string, string>? Settings { get; set; } = [];

        /// <summary>
        /// Configuration references associated with the vectorization profile.
        /// </summary>
        [JsonPropertyName("configuration_references")]
        public Dictionary<string, string>? ConfigurationReferences { get; set; } = [];
    }
}
