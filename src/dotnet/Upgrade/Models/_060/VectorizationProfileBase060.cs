﻿using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Basic properties for vectorization profiles.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(ContentSourceProfile060), "content-source-profile")]
    [JsonDerivedType(typeof(TextPartitioningProfile060), "text-partitioning-profile")]
    [JsonDerivedType(typeof(TextEmbeddingProfile060), "text-embedding-profile")]
    [JsonDerivedType(typeof(IndexingProfile060), "indexing-profile")]
    public class VectorizationProfileBase060 : ResourceBase060
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
