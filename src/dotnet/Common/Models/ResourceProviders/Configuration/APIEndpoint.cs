using FoundationaLLM.Common.Constants.ResourceProviders;

namespace FoundationaLLM.Common.Models.ResourceProviders.Configuration
{
    /// <summary>
    /// Represents an api endpoint resource.
    /// </summary>
    public class APIEndpoint : ResourceBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="APIEndpoint"/>.
        /// </summary>
        public APIEndpoint() =>
            Type = ConfigurationTypes.APIEndpoint;

        /// <summary>
        /// The type of authentication required for accessing the API.
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// The base URL of the API endpoint.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A list of URL exceptions.
        /// </summary>
        public List<UrlException> UrlExceptions { get; set; } = new List<UrlException>();

        /// <summary>
        /// The API key used for authentication.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The api key configuration name.
        /// </summary>
        public string ApiKeyConfigurationName { get; set; }

        /// <summary>
        /// The timeout duration in seconds for API calls.
        /// </summary>
        public int TimeoutSeconds { get; set; }

        /// <summary>
        /// The name of the retry strategy.
        /// </summary>
        public string RetryStrategyName { get; set; }
    }

    /// <summary>
    /// Represents an exception to the base URL.
    /// </summary>
    public class UrlException
    {
        /// <summary>
        /// The user principal name.
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// The alternative URL.
        /// </summary>
        public string Url { get; set; }
    }
}
