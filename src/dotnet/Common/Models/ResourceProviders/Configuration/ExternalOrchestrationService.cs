using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Configuration
{
    /// <summary>
    /// Represents an external orchestration service.
    /// </summary>
    public class ExternalOrchestrationService : ResourceBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalOrchestrationService"/>.
        /// </summary>
        public ExternalOrchestrationService() =>
            Type = ConfigurationTypes.ExternalOrchestrationService;

        /// <summary>
        /// The name of the app configuration key that contains the API URL.
        /// </summary>
        [JsonPropertyName("api_url_configuration_name")]
        public required string APIUrlConfigurationName { get; set; }

        /// <summary>
        /// The name of the app configuration key that contains the API key.
        /// </summary>
        [JsonPropertyName("api_key_configuration_name")]
        public required string APIKeyConfigurationName { get; set; }
    }
}
