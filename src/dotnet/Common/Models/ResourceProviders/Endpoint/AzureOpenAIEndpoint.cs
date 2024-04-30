using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// Azure OpenAI endpoint.
    /// </summary>
    public class AzureOpenAIEndpoint : EndpointBase
    {
        /// <summary>
        /// The API version to use for the endpoint.
        /// </summary>
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;

        /// <summary>
        /// The provider of the endpoint.
        /// </summary>
        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the <see cref="AzureOpenAIEndpoint"/> endpoint.
        /// </summary>
        public AzureOpenAIEndpoint() =>
            Type = EndpointTypes.AzureOpenAI;
    }
}
