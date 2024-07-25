using FoundationaLLM.Common.Constants.Authentication;
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
        /// A <see cref="AuthenticationTypes"/> value indicating the type of authentication used.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required AuthenticationTypes AuthenticationType { get; set; }

        /// <summary>
        /// The Azure Event Grid namespace endpoint.
        /// </summary>
        public string? Endpoint { get; set; }

        /// <summary>
        /// The API key used for authentication.
        /// This value should be set only if AuthenticationType has a value of APIKey.
        /// </summary>
        public string? APIKey { get; set; }

        /// <summary>
        /// The Azure resource identifier for the Azure Event Grid namespace.
        /// </summary>
        public required string NamespaceId { get; set; }
    }
}
