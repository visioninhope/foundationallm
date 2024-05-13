using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._040
{
    public class MetadataBase040
    {
        /// <summary>
        /// Name property.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Type property.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Description property.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Description property.
        /// </summary>
        [JsonPropertyName("version")]
        public Version? Version { get; set; }
    }
}
