using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// OpenAI endpoint.
    /// </summary>
    public class OpenAIEndpoint : EndpointBase
    {
        /// <summary>
        /// The provider of the endpoint.
        /// </summary>
        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the <see cref="OpenAIEndpoint"/> endpoint.
        /// </summary>
        public OpenAIEndpoint() =>
            Type = EndpointTypes.OpenAI;
    }
}
