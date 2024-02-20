using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Basic properties for vectorization profiles.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
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
        public string? Type { get; set; }

        /// <summary>
        /// The name of the vectorization profile.
        /// </summary>
        [JsonPropertyName("Name")]
        public required string Name { get; set; }

        /// <summary>
        /// The unique identifier of the object.
        /// </summary>
        [JsonPropertyName("ObjectId")]
        public string? ObjectId { get; set; }

        /// <summary>
        /// The configuration associated with the vectorization profile.
        /// </summary>
        [JsonPropertyName("Settings")]
        public Dictionary<string, string>? Settings { get; set; } = [];

        /// <summary>
        /// Configuration references associated with the vectorization profile.
        /// </summary>
        [JsonPropertyName("ConfigurationReferences")]
        public Dictionary<string, string>? ConfigurationReferences { get; set; } = [];
    }
}
