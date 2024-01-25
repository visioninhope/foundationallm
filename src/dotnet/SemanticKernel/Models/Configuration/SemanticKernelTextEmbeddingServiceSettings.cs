using FoundationaLLM.Common.Settings;
using System.Text.Json.Serialization;

namespace FoundationaLLM.SemanticKernel.Core.Models.Configuration
{
    /// <summary>
    /// Provides configuration settings for the <see cref="SemanticKernelTextEmbeddingService"/> service.
    /// </summary>
    /// <param name="DeploymentName">The name of the Azure Open AI deployment.</param>
    /// <param name="Endpoint">The endpoint of the Azure Open AI deployment.</param>
    /// <param name="APIKey">The API key used to connect to the Azure Open AI endpoint. Valid only if AuthenticationType is APIKey.</param>
    public record SemanticKernelTextEmbeddingServiceSettings(
        string DeploymentName,
        string Endpoint,
        string? APIKey)
    {
        /// <summary>
        /// The <see cref="AuthenticationType"/> indicating which authentication mechanism to use.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required AzureOpenAIAuthenticationTypes AuthenticationType {  get; set; } 
    }
}
