using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Constants.Authentication;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Configuration
{
    /// <summary>
    /// Represents an api endpoint resource.
    /// </summary>
    public class APIEndpointConfiguration : ResourceBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="APIEndpointConfiguration"/>.
        /// </summary>
        public APIEndpointConfiguration() =>
            Type = ConfigurationTypes.APIEndpointConfiguration;

        /// <summary>
        /// The api endpoint category.
        /// </summary>
        [JsonPropertyName("category")]
        public required APIEndpointCategory Category { get; set; }

        /// <summary>
        /// The type of authentication required for accessing the API.
        /// </summary>
        [JsonPropertyName("authentication_type")]
        public required AuthenticationTypes AuthenticationType { get; set; }

        /// <summary>
        /// The base URL of the API endpoint.
        /// </summary>
        [JsonPropertyName("url")]
        public required string Url { get; set; }

        /// <summary>
        /// A list of URL exceptions.
        /// </summary>
        [JsonPropertyName("url_exceptions")]
        public List<UrlException> UrlExceptions { get; set; } = new List<UrlException>();

        /// <summary>
        /// Dictionary of settings for authenticating that support the AuthenticationType 
        /// </summary>
        public Dictionary<string, object> AuthenticationParameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The timeout duration in seconds for API calls.
        /// </summary>
        [JsonPropertyName("timeout_seconds")]
        public required int TimeoutSeconds { get; set; }

        /// <summary>
        /// The name of the retry strategy.
        /// </summary>
        [JsonPropertyName("retry_strategy_name")]
        public required string RetryStrategyName { get; set; }

        /// <summary>
        /// The API provider
        /// </summary>
        public string? Provider { get; set; }

        /// <summary>
        /// The version of the API to call
        /// </summary>
        public string? APIVersion { get; set; }

        /// <summary>
        /// Type of operation the endpoint is performing.
        /// This value should be completions or chat.
        /// Default value is chat.
        /// </summary>
        public string? OperationType { get; set; }

    }

    /// <summary>
    /// Represents an exception to the base URL.
    /// </summary>
    public class UrlException
    {
        /// <summary>
        /// The user principal name.
        /// </summary>
        [JsonPropertyName("user_principal_name")]
        public required string UserPrincipalName { get; set; }

        /// <summary>
        /// The alternative URL.
        /// </summary>
        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }
}
