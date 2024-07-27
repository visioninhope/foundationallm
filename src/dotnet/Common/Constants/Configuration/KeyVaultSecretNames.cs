namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all Azure Key vault secret names referred by the Azure App Configuration keys.
    /// </summary>
    public static partial class KeyVaultSecretNames
    {
        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Core API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Core API to authenticate.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_ClientSecret =
            "foundationallm-apiendpoints-coreapi-configuration-entra-clientsecret";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Core Worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreWorker_APIKey =
            "foundationallm-apiendpoints-coreworker-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Core Worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreWorker_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Gatekeeper API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_APIKey =
            "foundationallm-apiendpoints-gatekeeperapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Gatekeeper Integration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperIntegrationAPI_APIKey =
            "foundationallm-apiendpoints-gatekeeperintergrationapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gatekeeper Integration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperIntegrationAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Orchestration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_OrchestrationAPI_APIKey =
            "foundationallm-apiendpoints-orchestrationapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Orchestration API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_OrchestrationAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the LangChain API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LangChainAPI_APIKey =
            "foundationallm-apiendpoints-langchainapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the LangChain API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LangChainAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Semantic Kernel API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_SemanticKernelAPI_APIKey =
            "foundationallm-apiendpoints-semantickernelapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Semantic Kernel API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_SemanticKernelAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Management API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Management API to authenticate.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_ClientSecret =
            "foundationallm-apiendpoints-managementapi-configuration-entra-clientsecret";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Vectorization API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationAPI_APIKey =
            "foundationallm-apiendpoints-vectorizationapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Vectorization API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Vectorization Worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationWorker_APIKey =
            "foundationallm-apiendpoints-vectorizationworker-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Vectorization worker service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationWorker_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Gateway API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI_APIKey =
            "foundationallm-apiendpoints-gatewayapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gateway API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Gateway Adapter API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAdapterAPI_APIKey =
            "foundationallm-apiendpoints-gatewayadapterapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gateway Adapter API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAdapterAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the State API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI_APIKey =
            "foundationallm-apinedpoints-stateapi-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the State API.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI_AppInsightsConnectionString =
            "foundationallm-appinsights-connection-string";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Azure OpenAI service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureOpenAI_APIKey =
            "foundationallm-apiendpoints-azureopenai-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Azure Event Grid service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_APIKey =
            "foundationallm-apiendpoints-azureeventgrid-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the API key for the Azure Content Safety service.
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_APIKey =
            "foundationallm-apiendpoints-azurecontentsafety-apikey";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the User Portal to authenticate.
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_ClientSecret =
            "foundationallm-userportal-authentication-entra-clientsecret";

        /// <summary>
        /// The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Management Portal to authenticate.
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_ClientSecret =
            "foundationallm-managementportal-authentication-entra-clientsecret";
    }
}
