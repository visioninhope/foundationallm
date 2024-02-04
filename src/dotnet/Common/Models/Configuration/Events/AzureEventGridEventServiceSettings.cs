using FoundationaLLM.Common.Services.Events;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Configuration.Events
{
    /// <summary>
    /// Provides configuration settings to intialize the <see cref="AzureEventGridEventService"/> service.
    /// </summary>
    public class AzureEventGridEventServiceSettings
    {
        /// <summary>
        /// A <see cref="AzureEventGridAuthenticationTypes"/> value indicating the type of authentication used.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required AzureEventGridAuthenticationTypes AuthenticationType { get; set; }

        /// <summary>
        /// The Azure Event Grid namespace endpoint.
        /// </summary>
        public string? Endpoint { get; set; }

        /// <summary>
        /// The API key used for authentication.
        /// This value should be set only if AuthenticationType has a value of APIKey.
        /// </summary>
        public string? APIKey { get; set; }
    }
}
