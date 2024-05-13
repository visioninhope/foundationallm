using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._040
{
    public class ResourceBase040 : ResourceName040
    {
        /// <summary>
        /// The unique identifier of the resource.
        /// </summary>
        [JsonPropertyName("object_id")]
        [JsonPropertyOrder(-4)]
        public string? ObjectId { get; set; }

        /// <summary>
        /// The description of the resource.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonPropertyOrder(-3)]
        public string? Description { get; set; }

        /// <summary>
        /// The version of the resource.
        /// </summary>
        [JsonPropertyName("version")]
        [JsonPropertyOrder(-3)]
        public Version? Version { get; set; } = new Version("0.4.0");

        /// <summary>
        /// Indicates whether the resource has been logically deleted.
        /// </summary>
        [JsonPropertyName("deleted")]
        [JsonPropertyOrder(-3)]
        public bool Deleted { get; set; } = false;
    }
}
