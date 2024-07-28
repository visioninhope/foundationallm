namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all configuration section names used to map configuration values to settings classes.
    /// </summary>
    public static partial class AppConfigurationKeySections
    {        
        /// <summary>
        /// Configuration section used to identify the settings related to the FoundationaLLM instance.
        /// </summary>
        public const string FoundationaLLM_Instance =
            "FoundationaLLM:Instance";
        
        /// <summary>
        /// Configuration section used to identify the Azure Key Vault settings related to the FoundationaLLM instance.
        /// </summary>
        public const string FoundationaLLM_Configuration =
            "FoundationaLLM:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.AIModel resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_AIModel_Storage =
            "FoundationaLLM:ResourceProviders:AIModel:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Agent resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Agent_Storage =
            "FoundationaLLM:ResourceProviders:Agent:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Attachment resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Attachment_Storage =
            "FoundationaLLM:ResourceProviders:Attachment:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Configuration resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Configuration_Storage =
            "FoundationaLLM:ResourceProviders:Configuration:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.DataSource resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_DataSource_Storage =
            "FoundationaLLM:ResourceProviders:DataSource:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Prompt resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Prompt_Storage =
            "FoundationaLLM:ResourceProviders:Prompt:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Vectorization resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Vectorization_Storage =
            "FoundationaLLM:ResourceProviders:Vectorization:Storage";
        
        /// <summary>
        /// Configuration section used to identify the settings for all API endpoints.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints =
            "FoundationaLLM:APIEndpoints";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Authorization API. Due to its special nature, the Authorization API does not have a corresponding APIEndpointConfiguration resource.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AuthorizationAPI =
            "FoundationaLLM:APIEndpoints:AuthorizationAPI";
        
        /// <summary>
        /// Configuration section used to identify the main Core API settings.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the Entra ID authentication settings for Core API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra";
        
        /// <summary>
        /// Configuration section used to identify the Cosmos DB settings for the Core API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Core Worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreWorker =
            "FoundationaLLM:APIEndpoints:CoreWorker";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Gatekeeper API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Gatekeeper API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_Configuration =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Gatekeeper Integration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperIntegrationAPI =
            "FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Orchestration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_OrchestrationAPI =
            "FoundationaLLM:APIEndpoints:OrchestrationAPI";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the LangChain API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LangChainAPI =
            "FoundationaLLM:APIEndpoints:LangChainAPI";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Semantic Kernel API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_SemanticKernelAPI =
            "FoundationaLLM:APIEndpoints:SemanticKernelAPI";
        
        /// <summary>
        /// Configuration section used to identify the Entra ID authentication settings for Management API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Vectorization API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationAPI =
            "FoundationaLLM:APIEndpoints:VectorizationAPI";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Vectorization Worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationWorker =
            "FoundationaLLM:APIEndpoints:VectorizationWorker";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Gateway API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI =
            "FoundationaLLM:APIEndpoints:GatewayAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Gateway API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI_Configuration =
            "FoundationaLLM:APIEndpoints:GatewayAPI:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the Gateway Adapter API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAdapterAPI =
            "FoundationaLLM:APIEndpoints:GatewayAdapterAPI";
        
        /// <summary>
        /// Configuration section used to identify the authentication settings for the State API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI =
            "FoundationaLLM:APIEndpoints:StateAPI";
        
        /// <summary>
        /// Configuration section used to identify the Cosmos DB settings for the State API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI_Configuration_CosmosDB =
            "FoundationaLLM:APIEndpoints:StateAPI:Configuration:CosmosDB";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure AI Search vector store service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureAISearchVectorStore_Configuration =
            "FoundationaLLM:APIEndpoints:AzureAISearchVectorStore:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure Cosmos DB NoSQL vector store service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureCosmosDBNoSQLVectorStore_Configuration =
            "FoundationaLLM:APIEndpoints:AzureCosmosDBNoSQLVectorStore:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure PostgreSQL vector store service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzurePostgreSQLVectorStore_Configuration =
            "FoundationaLLM:APIEndpoints:AzurePostgreSQLVectorStore:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure Event Grid service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure AI Studio service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureAIStudio_Configuration =
            "FoundationaLLM:APIEndpoints:AzureAIStudio:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for storage account used by the Azure AI Studio service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureAIStudio_Configuration_Storage =
            "FoundationaLLM:APIEndpoints:AzureAIStudio:Configuration:Storage";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Azure Content Safety service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_Configuration =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Lakera Guard service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LakeraGuard_Configuration =
            "FoundationaLLM:APIEndpoints:LakeraGuard:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the settings for the Encrypt Guardrails service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_EnkryptGuardrails_Configuration =
            "FoundationaLLM:APIEndpoints:EnkryptGuardrails:Configuration";
        
        /// <summary>
        /// Configuration section used to identify the branding settings for the User Portal.
        /// </summary>
        public const string FoundationaLLM_Branding =
            "FoundationaLLM:Branding";
        
        /// <summary>
        /// Configuration section used to identify the settings for vectorization steps.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Steps =
            "FoundationaLLM:Vectorization:Steps";
        
        /// <summary>
        /// Configuration section used to identify the settings for vectorization queues.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues =
            "FoundationaLLM:Vectorization:Queues";
        
        /// <summary>
        /// Configuration section used to identify the settings for the storage account used by the vectorization state service.
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService_Storage =
            "FoundationaLLM:Vectorization:StateService:Storage";
        
        /// <summary>
        /// Configuration section used to identify the settings for data sources resources managed by the FoundationaLLM.DataSource resource provider.
        /// </summary>
        public const string FoundationaLLM_DataSources =
            "FoundationaLLM:DataSources";
        
        /// <summary>
        /// Configuration section used to identify the settings for the events infrastructure used by the Core API.
        /// </summary>
        public const string FoundationaLLM_Events_Profiles_CoreAPI =
            "FoundationaLLM:Events:Profiles:CoreAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the events infrastructure used by the Orchestration API.
        /// </summary>
        public const string FoundationaLLM_Events_Profiles_OrchestrationAPI =
            "FoundationaLLM:Events:Profiles:OrchestrationAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the events infrastructure used by the Management API.
        /// </summary>
        public const string FoundationaLLM_Events_Profiles_ManagementAPI =
            "FoundationaLLM:Events:Profiles:ManagementAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the events infrastructure used by the Vectorization API.
        /// </summary>
        public const string FoundationaLLM_Events_Profiles_VectorizationAPI =
            "FoundationaLLM:Events:Profiles:VectorizationAPI";
        
        /// <summary>
        /// Configuration section used to identify the settings for the events infrastructure used by the Vectorization Worker service.
        /// </summary>
        public const string FoundationaLLM_Events_Profiles_VectorizationWorker =
            "FoundationaLLM:Events:Profiles:VectorizationWorker";
    }
}
