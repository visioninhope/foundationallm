using FoundationaLLM.Common.Models.Configuration.Storage;

namespace FoundationaLLM.Common.Models.Configuration.AzureAI
{
    /// <summary>
    /// Settings for submitting jobs to Azure AI Studio
    /// </summary>
    public class AzureAISettings
    {
        /// <summary>
        /// The storage backing the Azure AI Studio.
        /// </summary>
        public BlobStorageServiceSettings BlobStorageServiceSettings { get; init; }
        /// <summary>
        /// Base url of the Azure AI studio API.
        /// </summary>

        public string? BaseUrl { get; init; }
        /// <summary>
        /// Container where Azure AI Studio stores the data sets.
        /// </summary>
        public string? ContainerName { get; init; }
        /// <summary>
        /// Subscription Id of the Azure AI Studio.
        /// </summary>
        public string? SubscriptionId { get; init; }
        /// <summary>
        /// Region of the Azure AI Studio.
        /// </summary>
        public string? Region { get; init; }
        /// <summary>
        /// Resource Group of the Azure AI Studio.
        /// 
        public string? ResourceGroup { get; init; }
        /// <summary>
        /// Project Name of the Azure AI Studio.
        /// </summary>
        public string? ProjectName { get; init; }
        /// <summary>
        /// Deployment of the Azure AI Studio.
        /// </summary>
        public string? Deployment { get; init; }
        /// <summary>
        /// Metrics to run on the Azure AI Studio.
        /// 
        public string? Metrics { get; init; }
        /// <summary>
        /// The Flow Definition Resource Id of the Azure AI Studio.
        /// 
        public string? FlowDefinitionResourceId { get; init; }
    }
}
