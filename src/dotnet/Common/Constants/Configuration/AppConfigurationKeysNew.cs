namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all App Configuration key names used by FoundationaLLM.
    /// </summary>
    public static class AppConfigurationKeysNew
    {
        #region FoundationaLLM:Instance
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Instance:Id setting.
        /// <para>Value description:<br/>The unique identifier of the FoundationaLLM instance.</para>
        /// </summary>
        public const string FoundationaLLM_Instance_Id =
            "FoundationaLLM:Instance:Id";

        #endregion

        #region FoundationaLLM:Configuration
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:KeyVaultURI setting.
        /// <para>Value description:<br/>The URL of the Azure Key Vault providing the secrets.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_KeyVaultURI =
            "FoundationaLLM:Configuration:KeyVaultURI";

        #endregion

        #region FoundationaLLM:AIModel:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:AIModel:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.AIModel resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_AIModel_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:AIModel:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:AIModel:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.AIModel resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_AIModel_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:AIModel:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:AIModel:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.AIModel resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_AIModel_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:AIModel:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:AIModel:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.AIModel resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_AIModel_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:AIModel:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:Agent:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Agent:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.Agent resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Agent_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Agent:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Agent:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.Agent resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Agent_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:Agent:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Agent:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.Agent resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Agent_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:Agent:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Agent:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.Agent resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Agent_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:Agent:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:Attachment:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Attachment:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.Attachment resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Attachment_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Attachment:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Attachment:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.Attachment resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Attachment_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:Attachment:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Attachment:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.Attachment resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Attachment_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:Attachment:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Attachment:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.Attachment resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Attachment_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:Attachment:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:Configuration:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.Configuration resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Configuration:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.Configuration resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:Configuration:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.Configuration resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:Configuration:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.Configuration resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:Configuration:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:DataSource:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:DataSource:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.DataSource resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_DataSource_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:DataSource:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:DataSource:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.DataSource resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_DataSource_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:DataSource:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:DataSource:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.DataSource resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_DataSource_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:DataSource:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:DataSource:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.DataSource resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_DataSource_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:DataSource:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:Prompt:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Prompt:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.Prompt resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Prompt_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Prompt:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Prompt:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.Prompt resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Prompt_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:Prompt:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Prompt:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.Prompt resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Prompt_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:Prompt:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Prompt:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.Prompt resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Prompt_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:Prompt:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:Vectorization:ResourceProviderService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the FoundationaLLM.Vectorization resource provider. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Vectorization:ResourceProviderService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:ResourceProviderService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the FoundationaLLM.Vectorization resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_ResourceProviderService_Storage_AccountName =
            "FoundationaLLM:Vectorization:ResourceProviderService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:ResourceProviderService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the FoundationaLLM.Vectorization resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_ResourceProviderService_Storage_AccountKey =
            "FoundationaLLM:Vectorization:ResourceProviderService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:ResourceProviderService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the FoundationaLLM.Vectorization resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_ResourceProviderService_Storage_ConnectionString =
            "FoundationaLLM:Vectorization:ResourceProviderService:Storage:ConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:AuthorizationAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AuthorizationAPI:APIUrl setting.
        /// <para>Value description:<br/>The URL of the Authorization API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AuthorizationAPI_APIUrl =
            "FoundationaLLM:APIEndpoints:AuthorizationAPI:APIUrl";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AuthorizationAPI:APIScope setting.
        /// <para>Value description:<br/>The scope required to get an access token for the Authorization API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AuthorizationAPI_APIScope =
            "FoundationaLLM:APIEndpoints:AuthorizationAPI:APIScope";

        #endregion

        #region FoundationaLLM:APIEndpoints:CoreAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:CoreAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:Instance setting.
        /// <para>Value description:<br/>The URL of the Entra ID instance used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_Instance =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:Instance";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:TenantId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID tenant used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_TenantId =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:TenantId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:Scopes setting.
        /// <para>Value description:<br/>The list of scopes exposed by the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_Scopes =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:Scopes";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:CallbackPath setting.
        /// <para>Value description:<br/>The path where the Entra ID instance will redirect during authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_CallbackPath =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:CallbackPath";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:ClientId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID app registration used by the Core API to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_ClientId =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:ClientId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:ClientSecret setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Core API to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Entra_ClientSecret =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Entra:ClientSecret";

        #endregion

        #region FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Gatekeeper
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Gatekeeper:BypassGatekeeper setting.
        /// <para>Value description:<br/>The flag that indicates whether the Core API should bypass or not the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_Gatekeeper_BypassGatekeeper =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:Gatekeeper:BypassGatekeeper";

        #endregion

        #region FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Endpoint setting.
        /// <para>Value description:<br/>The URL of the Azure Cosmos DB service used by the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB_Endpoint =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Endpoint";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Database setting.
        /// <para>Value description:<br/>The name of the Azure Cosmos DB database used by the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB_Database =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Database";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Containers setting.
        /// <para>Value description:<br/>The comma-separated list of database containers used by the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB_Containers =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:Containers";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:MonitoredContainers setting.
        /// <para>Value description:<br/>The comma-separated list of database containers that are monitored for changes using the change feed.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB_MonitoredContainers =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:MonitoredContainers";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:ChangeFeedLeaseContainer setting.
        /// <para>Value description:<br/>The name of the container used by Azure Cosmos DB to manage the change feed leases.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB_ChangeFeedLeaseContainer =
            "FoundationaLLM:APIEndpoints:CoreAPI:Configuration:CosmosDB:ChangeFeedLeaseContainer";

        #endregion

        #region FoundationaLLM:APIEndpoints:CoreWorker
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreWorker:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Core Worker service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreWorker_APIKey =
            "FoundationaLLM:APIEndpoints:CoreWorker:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:CoreWorker:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Core Worker service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_CoreWorker_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:CoreWorker:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:GatekeeperAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_APIKey =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableLakeraGuard setting.
        /// <para>Value description:<br/>Indicates whether Lakera Guard is available for use by the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_Configuration_EnableLakeraGuard =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableLakeraGuard";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableEnkryptGuardrails setting.
        /// <para>Value description:<br/>Indicates whether Enkrypt Guardrails is available for use by the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_Configuration_EnableEnkryptGuardrails =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableEnkryptGuardrails";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableAzureContentSafetyPromptShields setting.
        /// <para>Value description:<br/>Indicates whether Azure Content Safety Prompt Shields is available for use by the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_Configuration_EnableAzureContentSafetyPromptShields =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableAzureContentSafetyPromptShields";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableMicrosoftPresidio setting.
        /// <para>Value description:<br/>Indicates whether Microsoft Presidio is available for use by the Gatekeeper API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperAPI_Configuration_EnableMicrosoftPresidio =
            "FoundationaLLM:APIEndpoints:GatekeeperAPI:Configuration:EnableMicrosoftPresidio";

        #endregion

        #region FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Gatekeeper Integration API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperIntegrationAPI_APIKey =
            "FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gatekeeper Integration API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatekeeperIntegrationAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:GatekeeperIntegrationAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:OrchestrationAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:OrchestrationAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Orchestration API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_OrchestrationAPI_APIKey =
            "FoundationaLLM:APIEndpoints:OrchestrationAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:OrchestrationAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Orchestration API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_OrchestrationAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:OrchestrationAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:LangChainAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:LangChainAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the LangChain API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LangChainAPI_APIKey =
            "FoundationaLLM:APIEndpoints:LangChainAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:LangChainAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the LangChain API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LangChainAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:LangChainAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:SemanticKernelAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:SemanticKernelAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Semantic Kernel API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_SemanticKernelAPI_APIKey =
            "FoundationaLLM:APIEndpoints:SemanticKernelAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:SemanticKernelAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Semantic Kernel API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_SemanticKernelAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:SemanticKernelAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:ManagementAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Management API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_APIKey =
            "FoundationaLLM:APIEndpoints:ManagementAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Management API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:ManagementAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:Instance setting.
        /// <para>Value description:<br/>The URL of the Entra ID instance used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_Instance =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:Instance";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:TenantId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID tenant used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_TenantId =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:TenantId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:Scopes setting.
        /// <para>Value description:<br/>The list of scopes exposed by the Management API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_Scopes =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:Scopes";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:CallbackPath setting.
        /// <para>Value description:<br/>The path where the Entra ID instance will redirect during authentication.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_CallbackPath =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:CallbackPath";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:ClientId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID app registration used by the Management API to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_ClientId =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:ClientId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:ClientSecret setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Management API to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_ClientSecret =
            "FoundationaLLM:APIEndpoints:ManagementAPI:Configuration:Entra:ClientSecret";

        #endregion

        #region FoundationaLLM:APIEndpoints:VectorizationAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:VectorizationAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Vectorization API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationAPI_APIKey =
            "FoundationaLLM:APIEndpoints:VectorizationAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:VectorizationAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Vectorization API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:VectorizationAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:VectorizationWorker
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:VectorizationWorker:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Vectorization Worker service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationWorker_APIKey =
            "FoundationaLLM:APIEndpoints:VectorizationWorker:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:VectorizationWorker:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Vectorization worker service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_VectorizationWorker_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:VectorizationWorker:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:GatewayAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatewayAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Gateway API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI_APIKey =
            "FoundationaLLM:APIEndpoints:GatewayAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatewayAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gateway API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:GatewayAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:GatewayAdapterAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatewayAdapterAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Gateway Adapter API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAdapterAPI_APIKey =
            "FoundationaLLM:APIEndpoints:GatewayAdapterAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:GatewayAdapterAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the Gateway Adapter API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_GatewayAdapterAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:GatewayAdapterAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:StateAPI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:StateAPI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the State API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI_APIKey =
            "FoundationaLLM:APIEndpoints:StateAPI:APIKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:StateAPI:AppInsightsConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the App Insights service used by the State API.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_StateAPI_AppInsightsConnectionString =
            "FoundationaLLM:APIEndpoints:StateAPI:AppInsightsConnectionString";

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureOpenAI
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureOpenAI:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Azure OpenAI service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureOpenAI_APIKey =
            "FoundationaLLM:APIEndpoints:AzureOpenAI:APIKey";

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureAISearch

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureEventGrid
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Azure Event Grid service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_APIKey =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:APIKey";

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:NamespaceId setting.
        /// <para>Value description:<br/>The object identifier of the Azure Event Grid Namespace object used by the Azure Event Grid event service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_NamespaceId =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:NamespaceId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:CoreAPI setting.
        /// <para>Value description:<br/>The settings used by the Core API to process Azure Event Grid events.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_Profiles_CoreAPI =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:CoreAPI";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:OrchestrationAPI setting.
        /// <para>Value description:<br/>The settings used by the Orchestration API to process Azure Event Grid events.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_Profiles_OrchestrationAPI =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:OrchestrationAPI";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:ManagementAPI setting.
        /// <para>Value description:<br/>The settings used by the Management API to process Azure Event Grid events.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_Profiles_ManagementAPI =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:ManagementAPI";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:Vectorization setting.
        /// <para>Value description:<br/>The settings used by the Vectorization API to process Azure Event Grid events.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_Profiles_Vectorization =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:Vectorization";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:VectorizationWorker setting.
        /// <para>Value description:<br/>The settings used by the Vectorization Worker to process Azure Event Grid events.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureEventGrid_Configuration_Profiles_VectorizationWorker =
            "FoundationaLLM:APIEndpoints:AzureEventGrid:Configuration:Profiles:VectorizationWorker";

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureContentSafety
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureContentSafety:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Azure Content Safety service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_APIKey =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:APIKey";

        #endregion

        #region FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:HateSeverity setting.
        /// <para>Value description:<br/>The maximum level of hate language that is allowed by the Azure Content Safety service (higher value means that a more severe language is allowed).</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_Configuration_HateSeverity =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:HateSeverity";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:SelfHarmSeverity setting.
        /// <para>Value description:<br/>The maximum level of self-harm language that is allowed by the Azure Content Safety service (higher value means that a more severe language is allowed).</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_Configuration_SelfHarmSeverity =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:SelfHarmSeverity";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:SexualSeverity setting.
        /// <para>Value description:<br/>The maximum level of sexual language that is allowed by the Azure Content Safety service (higher value means that a more severe language is allowed).</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_Configuration_SexualSeverity =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:SexualSeverity";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:ViolenceSeverity setting.
        /// <para>Value description:<br/>The maximum level of violent language that is allowed by the Azure Content Safety service (higher value means that a more severe language is allowed).</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_AzureContentSafety_Configuration_ViolenceSeverity =
            "FoundationaLLM:APIEndpoints:AzureContentSafety:Configuration:ViolenceSeverity";

        #endregion

        #region FoundationaLLM:APIEndpoints:LakeraGuard
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:LakeraGuard:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Lakera Guard service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_LakeraGuard_APIKey =
            "FoundationaLLM:APIEndpoints:LakeraGuard:APIKey";

        #endregion

        #region FoundationaLLM:APIEndpoints:EnkryptGuardrails
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:APIEndpoints:EnkryptGuardrails:APIKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the API key for the Enkrypt Guardrails service.</para>
        /// </summary>
        public const string FoundationaLLM_APIEndpoints_EnkryptGuardrails_APIKey =
            "FoundationaLLM:APIEndpoints:EnkryptGuardrails:APIKey";

        #endregion

        #region FoundationaLLM:Branding
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:AccentColor setting.
        /// <para>Value description:<br/>Accent color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_AccentColor =
            "FoundationaLLM:Branding:AccentColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:AccentTextColor setting.
        /// <para>Value description:<br/>Accent text color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_AccentTextColor =
            "FoundationaLLM:Branding:AccentTextColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:BackgroundColor setting.
        /// <para>Value description:<br/>BackgroundColor.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_BackgroundColor =
            "FoundationaLLM:Branding:BackgroundColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:CompanyName setting.
        /// <para>Value description:<br/>Company name.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_CompanyName =
            "FoundationaLLM:Branding:CompanyName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:FavIconUrl setting.
        /// <para>Value description:<br/>Fav icon url.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_FavIconUrl =
            "FoundationaLLM:Branding:FavIconUrl";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:KioskMode setting.
        /// <para>Value description:<br/>Kiosk mode.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_KioskMode =
            "FoundationaLLM:Branding:KioskMode";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:LogoText setting.
        /// <para>Value description:<br/>Logo text.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_LogoText =
            "FoundationaLLM:Branding:LogoText";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:LogoUrl setting.
        /// <para>Value description:<br/>Logo url.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_LogoUrl =
            "FoundationaLLM:Branding:LogoUrl";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:PageTitle setting.
        /// <para>Value description:<br/>Page title.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_PageTitle =
            "FoundationaLLM:Branding:PageTitle";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:PrimaryColor setting.
        /// <para>Value description:<br/>Primary color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_PrimaryColor =
            "FoundationaLLM:Branding:PrimaryColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:PrimaryTextColor setting.
        /// <para>Value description:<br/>Primary text color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_PrimaryTextColor =
            "FoundationaLLM:Branding:PrimaryTextColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:SecondaryColor setting.
        /// <para>Value description:<br/>Secondary color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_SecondaryColor =
            "FoundationaLLM:Branding:SecondaryColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:SecondaryTextColor setting.
        /// <para>Value description:<br/>Secondary text color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_SecondaryTextColor =
            "FoundationaLLM:Branding:SecondaryTextColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:PrimaryButtonBackgroundColor setting.
        /// <para>Value description:<br/>Primary button background color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_PrimaryButtonBackgroundColor =
            "FoundationaLLM:Branding:PrimaryButtonBackgroundColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:PrimaryButtonTextColor setting.
        /// <para>Value description:<br/>Primary button text color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_PrimaryButtonTextColor =
            "FoundationaLLM:Branding:PrimaryButtonTextColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:SecondaryButtonBackgroundColor setting.
        /// <para>Value description:<br/>Secondary button background color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_SecondaryButtonBackgroundColor =
            "FoundationaLLM:Branding:SecondaryButtonBackgroundColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:SecondaryButtonTextColor setting.
        /// <para>Value description:<br/>Secondary button text color.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_SecondaryButtonTextColor =
            "FoundationaLLM:Branding:SecondaryButtonTextColor";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Branding:FooterText setting.
        /// <para>Value description:<br/>Footer text.</para>
        /// </summary>
        public const string FoundationaLLM_Branding_FooterText =
            "FoundationaLLM:Branding:FooterText";

        #endregion

        #region FoundationaLLM:UserPortal:Authentication:Entra
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:Instance setting.
        /// <para>Value description:<br/>The URL of the Entra ID instance used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_Instance =
            "FoundationaLLM:UserPortal:Authentication:Entra:Instance";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:TenantId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID tenant used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_TenantId =
            "FoundationaLLM:UserPortal:Authentication:Entra:TenantId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:Scopes setting.
        /// <para>Value description:<br/>The list of scopes used to get the authentication token for the Core API.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_Scopes =
            "FoundationaLLM:UserPortal:Authentication:Entra:Scopes";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:CallbackPath setting.
        /// <para>Value description:<br/>The path where the Entra ID instance will redirect during authentication.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_CallbackPath =
            "FoundationaLLM:UserPortal:Authentication:Entra:CallbackPath";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:ClientId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID app registration used by the User Portal to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_ClientId =
            "FoundationaLLM:UserPortal:Authentication:Entra:ClientId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:UserPortal:Authentication:Entra:ClientSecret setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the User Portal to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_UserPortal_Authentication_Entra_ClientSecret =
            "FoundationaLLM:UserPortal:Authentication:Entra:ClientSecret";

        #endregion

        #region FoundationaLLM:ManagementPortal:Authentication:Entra
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:Instance setting.
        /// <para>Value description:<br/>The URL of the Entra ID instance used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_Instance =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:Instance";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:TenantId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID tenant used for authentication.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_TenantId =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:TenantId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:Scopes setting.
        /// <para>Value description:<br/>The list of scopes used to get the authentication token for the Management API.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_Scopes =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:Scopes";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:CallbackPath setting.
        /// <para>Value description:<br/>The path where the Entra ID instance will redirect during authentication.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_CallbackPath =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:CallbackPath";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:ClientId setting.
        /// <para>Value description:<br/>The unique identifier of the Entra ID app registration used by the Management Portal to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_ClientId =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:ClientId";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:ManagementPortal:Authentication:Entra:ClientSecret setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the client secret associated with the Entra ID app registration used by the Management Portal to authenticate.</para>
        /// </summary>
        public const string FoundationaLLM_ManagementPortal_Authentication_Entra_ClientSecret =
            "FoundationaLLM:ManagementPortal:Authentication:Entra:ClientSecret";

        #endregion

        #region FoundationaLLM:Vectorization
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:Worker setting.
        /// <para>Value description:<br/>The processing configuration used by the Vectorization Worker service.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_Worker =
            "FoundationaLLM:Vectorization:Worker";

        #endregion

        #region FoundationaLLM:Vectorization:Queues
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:Queues:Extract:AccountName setting.
        /// <para>Value description:<br/>The Azure Queue Storage account providing the Extract queue.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues_Extract_AccountName =
            "FoundationaLLM:Vectorization:Queues:Extract:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:Queues:Partition:AccountName setting.
        /// <para>Value description:<br/>The Azure Queue Storage account providing the Partition queue.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues_Partition_AccountName =
            "FoundationaLLM:Vectorization:Queues:Partition:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:Queues:Embed:AccountName setting.
        /// <para>Value description:<br/>The Azure Queue Storage account providing the Embed queue.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues_Embed_AccountName =
            "FoundationaLLM:Vectorization:Queues:Embed:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:Queues:Index:AccountName setting.
        /// <para>Value description:<br/>The Azure Queue Storage account providing the Index queue.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_Queues_Index_AccountName =
            "FoundationaLLM:Vectorization:Queues:Index:AccountName";

        #endregion

        #region FoundationaLLM:Vectorization:StateService:Storage
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:StateService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the Azure Blob Storage account used by the Vectorization State service. Can be one of: AzureIdentity, AccountKey, or ConnectionString.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService_Storage_AuthenticationType =
            "FoundationaLLM:Vectorization:StateService:Storage:AuthenticationType";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:StateService:Storage:AccountName setting.
        /// <para>Value description:<br/>The name of the Azure Blob Storage account used by the Vectorization State service.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService_Storage_AccountName =
            "FoundationaLLM:Vectorization:StateService:Storage:AccountName";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:StateService:Storage:AccountKey setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the key for the Azure Blob Storage account used by the Vectorization State service.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService_Storage_AccountKey =
            "FoundationaLLM:Vectorization:StateService:Storage:AccountKey";
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Vectorization:StateService:Storage:ConnectionString setting.
        /// <para>Value description:<br/>The name of the Azure Key Vault secret holding the connection string for the Azure Blob Storage account used by the Vectorization State service.</para>
        /// </summary>
        public const string FoundationaLLM_Vectorization_StateService_Storage_ConnectionString =
            "FoundationaLLM:Vectorization:StateService:Storage:ConnectionString";

        #endregion
    }
}
