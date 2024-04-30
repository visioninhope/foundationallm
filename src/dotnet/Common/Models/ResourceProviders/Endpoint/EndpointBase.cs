using FoundationaLLM.Common.Constants.ResourceProviders;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// Basic endpoint.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(AzureAIEndpoint), EndpointTypes.AzureAI)]
    [JsonDerivedType(typeof(AzureOpenAIEndpoint), EndpointTypes.AzureOpenAI)]
    [JsonDerivedType(typeof(ExternalEndpoint), EndpointTypes.External)]
    [JsonDerivedType(typeof(OpenAIEndpoint), EndpointTypes.OpenAI)]
    public class EndpointBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }

        /// <summary>
        /// The endpoint URL.
        /// </summary>
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// The authentication type for the endpoint.
        /// The value can be one of the following: "key" or "token".
        /// </summary>
        [JsonPropertyName("authentication_type")]
        public string AuthenticationType { get; set; } = string.Empty;

        /// <summary>
        /// The API key for the endpoint, if the authentication type is "key".
        /// </summary>
        [JsonPropertyName("api_key")]
        public string? APIKey { get; set; }

        /// <summary>
        /// The operation type for the endpoint.
        /// Value can be either chat or completion.
        /// </summary>
        [JsonPropertyName("operation_type")]
        public string? OperationType { get; set; }

        /// <summary>
        /// Configuration references associated with the endpoint.
        /// </summary>
        [JsonPropertyName("configuration_references")]
        public Dictionary<string, string>? ConfigurationReferences { get; set; } = [];
    }
}
