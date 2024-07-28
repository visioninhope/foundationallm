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

}
