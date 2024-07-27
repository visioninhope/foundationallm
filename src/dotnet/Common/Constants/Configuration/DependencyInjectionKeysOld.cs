namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Contains constants of the keys for all keyed dependency injections.
    /// </summary>
    public static partial class DependencyInjectionKeys
    {
        /// <summary>
        /// The dependency injection key for the blob storage vectorization state service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_BlobStorageVectorizationStateService = "FoundationaLLM:Vectorization:BlobStorageVectorizationStateService";

        /// <summary>
        /// The dependency injection key for the vectorization data lake content source service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_DataLakeContentSourceService = "FoundationaLLM:Vectorization:DataLakeContentSourceService";

        /// <summary>
        /// The dependency injection key for the content source service factory.
        /// </summary>
        public const string FoundationaLLM_Vectorization_ContentSourceServiceFactory = "FoundationaLLM:Vectorization:ContentSourceServiceFactory";

        /// <summary>
        /// The dependency injection key for the Semantic Kernel text embedding service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_SemanticKernelTextEmbeddingService = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService";

        /// <summary>
        /// The dependency injection key for the Gateway text embedding service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_GatewayTextEmbeddingService = "FoundationaLLM:Vectorization:GatewayTextEmbeddingService";

        /// <summary>
        /// The dependency injection key for the Azure AI Search indexing service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_AzureAISearchIndexingService = "FoundationaLLM:Vectorization:AzureAISearchIndexingService";

        /// <summary>
        /// The dependency injection key for the Azure Cosmos DB NoSQL indexing service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_AzureCosmosDBNoSQLIndexingService = "FoundationaLLM:Vectorization:AzureCosmosDBNoSQLIndexingService";

        /// <summary>
        /// The dependency injection key for the PostgreSQL indexing service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_PostgresIndexingService = "FoundationaLLM:Vectorization:PostgresIndexingService";

        /// <summary>
        /// The dependency injection key for the vectorization queues configuration section.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues = "FoundationaLLM:Vectorization:Queues";

        /// <summary>
        /// The dependency injection key for the vectorization steps configuration section.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Steps = "FoundationaLLM:Vectorization:Steps";
    }
}
