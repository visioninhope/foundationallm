using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// AzureAI endpoint.
    /// </summary>
    public class AzureAIEndpoint : EndpointBase
    {
        /// <summary>
        /// The API version to use for the endpoint.
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the <see cref="AzureAIEndpoint"/> endpoint.
        /// </summary>
        public AzureAIEndpoint() =>
            Type = EndpointTypes.AzureAI;
    }
}
