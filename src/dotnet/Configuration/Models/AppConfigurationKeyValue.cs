using System.Text.Json.Serialization;

namespace FoundationaLLM.Configuration.Models
{
    /// <summary>
    /// Azure App Configuration key value model.
    /// </summary>
    public class AppConfigurationKeyValue : AppConfigurationKeyBase
    {
        /// <summary>
        /// The value of the App Configuration key.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// The content type of the value.
        /// </summary>
        [JsonPropertyName("content_type")]
        public string? ContentType { get; set; }
    }
}
