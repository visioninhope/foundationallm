using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.ResourceProviders.AIModel
{
    /// <summary>
    /// Defines the properties needed to call the endpoint for an AIModel
    /// </summary>
    public class AIModelEndpoint : ResourceBase
    {
        /// <summary>
        /// The URL for the endpoint
        /// </summary>
        [JsonPropertyName("endpoint_url")]
        public string? EndpointUrl { get; set; }
        /// <summary>
        /// Defines the authentication type
        /// </summary>
        [JsonPropertyName("authentication_type")]
        public string? AuthenticationType { get; set; }
        /// <summary>
        /// The parameters needed for completing authentication (i.e. IdP authority address, client id, etc.),
        /// typically loaded from App Config for the model provider
        /// </summary>
        [JsonPropertyName("authentication_parameters")]
        public Dictionary<string, object> AuthenticationParameters { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// Latest supported version of the endpoint being called
        /// </summary>
        [JsonPropertyName("api_version")]
        public string? APIVersion { get; set; }
        /// <summary>
        /// Name of the provider for the endpoint (i.e. AzureOpenAI, AzureAI, OpenAI, etc.)
        /// </summary>
        [JsonPropertyName("provider")]
        public string? Provider { get; set; }
        /// <summary>
        /// The operation type
        /// </summary>
        public string? OperationType { get; set; }
    }
}
