namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Contains constants of the keys filters for app configuration setting namespaces.
    /// </summary>
    public static class AppConfigurationKeyFilters
    {
        /// <summary>
        /// The key filter for the FoundationaLLM:Instance:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Instance = "FoundationaLLM:Instance:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Configuration:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Configuration = "FoundationaLLM:Configuration:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Branding:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Branding = "FoundationaLLM:Branding:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints = "FoundationaLLM:APIEndpoints:*";

        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:VectorizationAPI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_VectorizationAPI = "FoundationaLLM:APIs:VectorizationAPI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:VectorizationWorker:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_VectorizationWorker = "FoundationaLLM:APIs:VectorizationWorker:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:GatewayAPI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatewayAPI = "FoundationaLLM:APIs:GatewayAPI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:GatewayAdapterAPI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatewayAdapterAPI = "FoundationaLLM:APIs:GatewayAdapterAPI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:APIs:StateAPI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_StateAPI = "FoundationaLLM:APIs:StateAPI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:CosmosDB:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_CosmosDB = "FoundationaLLM:CosmosDB:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:CoreAPI:Entra:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_CoreAPI_Entra = "FoundationaLLM:CoreAPI:Entra:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:ManagementAPI:Entra:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_ManagementAPI_Entra = "FoundationaLLM:ManagementAPI:Entra:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Chat:Entra:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Chat_Entra = "FoundationaLLM:Chat:Entra:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Management:Entra:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Management_Entra = "FoundationaLLM:Management:Entra:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Orchestration:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Orchestration = "FoundationaLLM:Orchestration:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:CoreWorker:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_CoreWorker = "FoundationaLLM:CoreWorker:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Refinement:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Refinement = "FoundationaLLM:Refinement:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:AzureContentSafety:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureContentSafety = "FoundationaLLM:AzureContentSafety:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:BlobStorageMemorySource:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_BlobStorageMemorySource = "FoundationaLLM:CoreAPI:BlobStorageMemorySource:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Vectorization:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization = "FoundationaLLM:Vectorization:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Agent:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Agent = "FoundationaLLM:Agent:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Prompt:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Prompt = "FoundationaLLM:Prompt:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Prompt:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AIModel = "FoundationaLLM:AIModel:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Events:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events = "FoundationaLLM:Events:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:DataSource:* app configuration settings.
        /// This supports the DataSource resource provider settings.
        /// </summary>
        public const string FoundationaLLM_DataSource = "FoundationaLLM:DataSource:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:DataSources:* app configuration settings.
        /// This supports data source settings created by the Management API.
        /// </summary>
        public const string FoundationaLLM_DataSources = "FoundationaLLM:DataSources:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Attachment:* app configuration settings.
        /// This supports the Attachment resource provider settings.
        /// </summary>
        public const string FoundationaLLM_Attachment = "FoundationaLLM:Attachment:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:AzureOpenAI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureOpenAI = "FoundationaLLM:AzureOpenAI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:AzureAI:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureAI = "FoundationaLLM:AzureAI:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:Gateway:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Gateway = "FoundationaLLM:Gateway:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:State:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_State = "FoundationaLLM:State:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:AzureAIStudio:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureAIStudio = "FoundationaLLM:AzureAIStudio:*";
        /// <summary>
        /// The key filter for the FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:* app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureAIStudio_BlobStorageServiceSettings = "FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:*";
    }

    /// <summary>
    /// Contains constants of the keys sections for app configuration setting namespaces.
    /// </summary>
    public static partial class AppConfigurationKeySections
    {
        /// <summary>
        /// The key section for the FoundationaLLM:Instance app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Instance = "FoundationaLLM:Instance";
        /// <summary>
        /// The key section for the FoundationaLLM:Branding app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Branding = "FoundationaLLM:Branding";
        /// <summary>
        /// The key section for the FoundationaLLM:CosmosDB app configuration settings.
        /// </summary>
        public const string FoundationaLLM_CosmosDB = "FoundationaLLM:CosmosDB";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:CoreAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_CoreAPI = "FoundationaLLM:APIs:CoreAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:OrchestrationAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_OrchestrationAPI = "FoundationaLLM:APIs:OrchestrationAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:SemanticKernelAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_SemanticKernelAPI = "FoundationaLLM:APIs:SemanticKernelAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:LangChainAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_LangChainAPI = "FoundationaLLM:APIs:LangChainAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:AgentHubAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_AgentHubAPI = "FoundationaLLM:APIs:AgentHubAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:OrchestrationAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_PromptHubAPI = "FoundationaLLM:APIs:PromptHubAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:GatekeeperAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatekeeperAPI = "FoundationaLLM:APIs:GatekeeperAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:GatekeeperAPI:Configuration app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatekeeperAPI_Configuration = "FoundationaLLM:APIs:GatekeeperAPI:Configuration";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:VectorizationAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_VectorizationAPI = "FoundationaLLM:APIs:VectorizationAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:VectorizationWorker app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_VectorizationWorker = "FoundationaLLM:APIs:VectorizationWorker";

        /// <summary>
        /// The key section for the FoundationaLLM:APIs:GatewayAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatewayAPI = "FoundationaLLM:APIs:GatewayAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:GatewayAdapterAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_GatewayAdapterAPI = "FoundationaLLM:APIs:GatewayAdapterAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIs:StateAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_StateAPI = "FoundationaLLM:APIs:StateAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:APIEndpoints app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints = "FoundationaLLM:APIEndpoints";
        /// <summary>
        /// The key section for the FoundationaLLM:Orchestration app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Orchestration = "FoundationaLLM:Orchestration";
        /// <summary>
        /// The key section for the FoundationaLLM:SemanticKernelAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_SemanticKernelAPI = "FoundationaLLM:SemanticKernelAPI";
        /// <summary>
        /// The key section for the FoundationaLLM:Refinement app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Refinement = "FoundationaLLM:Refinement";
        /// <summary>
        /// The key section for the FoundationaLLM:AzureContentSafety app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_Gatekeeper_AzureContentSafety = "FoundationaLLM:APIs:Gatekeeper:AzureContentSafety";
        /// <summary>
        /// The key section for the FoundationaLLM:EnkryptGuardrails app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_Gatekeeper_EnkryptGuardrails = "FoundationaLLM:APIs:Gatekeeper:EnkryptGuardrails";
        /// <summary>
        /// The key section for the FoundationaLLM:LakeraGuard app configuration settings.
        /// </summary>
        public const string FoundationaLLM_APIs_Gatekeeper_LakeraGuard = "FoundationaLLM:APIs:Gatekeeper:LakeraGuard";
        /// <summary>
        /// The key section for the FoundationaLLM:BlobStorageMemorySource app configuration settings.
        /// </summary>
        public const string FoundationaLLM_BlobStorageMemorySource = "FoundationaLLM:BlobStorageMemorySource";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:Steps app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Steps = "FoundationaLLM:Vectorization:Steps";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:Queues app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues = "FoundationaLLM:Vectorization:Queues";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:StateService:Storage app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService = "FoundationaLLM:Vectorization:StateService:Storage";
        /// <summary>
        /// The key section for the FoundationaLLM:DataSources app configuration settings.
        /// </summary>
        public const string FoundationaLLM_DataSources = "FoundationaLLM:DataSources";
        /// <summary>
        /// The key section for the FoundationaLLM:Attachments app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Attachments = "FoundationaLLM:Attachments";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_SemanticKernelTextEmbeddingService = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:AzureAISearchIndexingService app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_AzureAISearchIndexingService = "FoundationaLLM:Vectorization:AzureAISearchIndexingService";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:AzureCosmosDBNoSQLIndexingService app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_AzureCosmosDBNoSQLIndexingService = "FoundationaLLM:Vectorization:AzureCosmosDBNoSQLIndexingService";
        /// <summary>
        /// The key section for the FoundationaLLM:Vectorization:PostgresIndexingService app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Vectorization_PostgresIndexingService = "FoundationaLLM:Vectorization:PostgresIndexingService";
        /// <summary>
        /// The key section for the FoundationaLLM:Gateway app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Gateway = "FoundationaLLM:Gateway";
        /// <summary>
        /// The key section for the FoundationaLLM:AzureAIStudio app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureAIStudio = "FoundationaLLM:AzureAIStudio";
        /// <summary>
        /// The key section for the FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AzureAIStudio_BlobStorageServiceSettings = "FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings";
        /// <summary>
        /// The key section for the FoundationaLLM:AIModels app configuration settings.
        /// </summary>
        public const string FoundationaLLM_AIModels = "FoundationaLLM:AIModels";
        /// The key section for the FoundationaLLM:State app configuration settings.
        /// </summary>
        public const string FoundationaLLM_State = "FoundationaLLM:State";

        #region Event Grid events

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService = "FoundationaLLM:Events:AzureEventGridEventService";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:CoreAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_CoreAPI = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:CoreAPI";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:OrchestrationAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_OrchestrationAPI = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:OrchestrationAPI";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:SemanticKernelAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_SemanticKernelAPI = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:SemanticKernelAPI";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:ManagementAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_ManagementAPI = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:ManagementAPI";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:VectorizationAPI app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_VectorizationAPI = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:VectorizationAPI";

        /// <summary>
        /// The key section for the FoundationaLLM:Events:AzureEventGridEventService:Profiles:VectorizationWorker app configuration settings.
        /// </summary>
        public const string FoundationaLLM_Events_AzureEventGridEventService_Profiles_VectorizationWorker = "FoundationaLLM:Events:AzureEventGridEventService:Profiles:VectorizationWorker";

        #endregion
    }
}
