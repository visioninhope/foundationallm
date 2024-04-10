namespace FoundationaLLM.Gateway.Models.Configuration
{
    /// <summary>
    /// Provides settings for the Gateway core service.
    /// </summary>
    public class GatewayServiceSettings
    {
        /// <summary>
        /// The semicolon separated list of Azure Open AI endpoints used by the Gateway service.
        /// </summary>
        public required string AzureOpenAIEndpoints { get; set; }
    }
}
