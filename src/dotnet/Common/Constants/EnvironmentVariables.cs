namespace FoundationaLLM.Common.Constants
{
    /// <summary>
    /// Contains constants for environment variables used by the application.
    /// </summary>
    public static class EnvironmentVariables
    {
        /// <summary>
        /// The client id of the user assigned managed identity.
        /// </summary>
        public const string AzureClientId = "AZURE_CLIENT_ID";

        /// <summary>
        /// The Azure Container App or Azure Kubernetes Service hostname.
        /// </summary>
        public const string Hostname = "HOSTNAME";
        /// <summary>
        /// The build version of the container. This is also used for the app version used
        /// to validate the minimum version of the app required to use certain configuration entries.
        /// </summary>
        public const string FoundationaLLM_Version = "FOUNDATIONALLM_VERSION";

        /// <summary>
        /// The key for the FoundationaLLM:AppConfig:ConnectionString environment variable.
        /// This allows the caller to connect to the Azure App Configuration service.
        /// </summary>
        public const string FoundationaLLM_AppConfig_ConnectionString = "FoundationaLLM:AppConfig:ConnectionString";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:AppInsightsConnectionString environment variable.
        /// This allows the caller to connect to OpenTelemetry and send telemetry data to Azure Monitor.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_AppInsightsConnectionString = "FoundationaLLM:AuthorizationAPI:AppInsightsConnectionString";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Entra:Instance environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Entra_Instance = "FoundationaLLM:AuthorizationAPI:Entra:Instance";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Entra:TenantId environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Entra_TenantId = "FoundationaLLM:AuthorizationAPI:Entra:TenantId";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Entra:ClientId environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Entra_ClientId = "FoundationaLLM:AuthorizationAPI:Entra:ClientId";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Entra:ClientSecret environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Entra_ClientSecret = "FoundationaLLM:AuthorizationAPI:Entra:ClientSecret";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Entra:Scopes environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Entra_Scopes = "FoundationaLLM:AuthorizationAPI:Entra:Scopes";

        /// <summary>
        /// The key for the FoundationaLLM:AuthorizationAPI:Storage:AccountName environment variable.
        /// </summary>
        public const string FoundationaLLM_AuthorizationAPI_Storage_AccountName = "FoundationaLLM:AuthorizationAPI:Storage:AccountName";
    }
}
