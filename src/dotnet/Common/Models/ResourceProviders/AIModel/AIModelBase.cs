using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.DataSource;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.AIModel
{
    /// <summary>
    /// Base model type for AIModel resources
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(EmbeddingAIModel), AIModelTypes.Embedding)]
    [JsonDerivedType(typeof(CompletionAIModel), AIModelTypes.Completion)]
    public class AIModelBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }
        /// <summary>
        /// The endpoint metadata needed to call the AI model endpoint
        /// </summary>
        [JsonPropertyName("endpoint")]
        public APIEndpointConfiguration? Endpoint { get; set; }
        /// <summary>
        /// The version for the AI model
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }
        /// <summary>
        /// Deployment name for the AI model
        [JsonPropertyName("deployment_name")]
        /// </summary>
        public string? DeploymentName { get; set; }
        /// <summary>
        /// Key value parameters configured for the model
        /// </summary>
        [JsonPropertyName("model_parameters")]
        public Dictionary<string, object> ModelParameters { get; set; } = new Dictionary<string, object>();

    }
}
