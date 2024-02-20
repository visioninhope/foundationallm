using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Basic properties for vectorization profiles.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(ContentSourceProfile), "content-source-profile")]
    [JsonDerivedType(typeof(TextPartitioningProfile), "text-partitioning-profile")]
    [JsonDerivedType(typeof(TextEmbeddingProfile), "text-embedding-profile")]
    [JsonDerivedType(typeof(IndexingProfile), "indexing-profile")]
    public class VectorizationProfileBase
    {
        /// <summary>
        /// The type of the vectorization profile.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// The name of the vectorization profile.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// The unique identifier of the object.
        /// </summary>
        [JsonPropertyName("object_id")]
        public string? ObjectId { get; set; }

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
