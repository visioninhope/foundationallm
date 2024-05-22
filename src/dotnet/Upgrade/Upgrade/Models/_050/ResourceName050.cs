using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    public class ResourceName050
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonPropertyOrder(-5)]
        public required string Name { get; set; }
        /// <summary>
        /// The type of the resource.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonPropertyOrder(-100)]
        public virtual string? Type { get; set; }
    }
}
