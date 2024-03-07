using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProvider
{
    /// <summary>
    /// Basic properties for all resources.
    /// </summary>
    public class ResourceBase : ResourceName
    {
        /// <summary>
        /// The unique identifier of the resource.
        /// </summary>
        [JsonPropertyName("object_id")]
        [JsonPropertyOrder(-4)]
        public string? ObjectId { get; set; }

        /// <summary>
        /// The display name of the resource.
        /// </summary>
        [JsonPropertyName("display_name")]
        [JsonPropertyOrder(-3)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// The description of the resource.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonPropertyOrder(-2)]
        public string? Description { get; set; }

        /// <summary>
        /// Indicates whether the resource has been logically deleted.
        /// </summary>
        [JsonPropertyName("deleted")]
        [JsonPropertyOrder(100)]
        public virtual bool Deleted { get; set; } = false;
    }
}
