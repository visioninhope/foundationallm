using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Models.Configuration.AppConfiguration;

namespace FoundationaLLM.Configuration.Catalog
{
    /// <summary>
    /// A catalog containing the configuration entries for the solution.
    /// </summary>

    public static class AppConfigCatalog
    {
        /// <summary>
        /// The configuration entries for the solution.
        /// </summary>
        private static readonly List<AppConfigEntry> Entries =
        [
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Instance_Id,
                minimumVersion: "1.0.0",
                defaultValue: "Generated GUID",
                description: "The value should be a GUID represents a unique instance of the FoundationaLLM instance.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AgentHub_AgentMetadata_StorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "agents",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AgentHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-agenthub-storagemanager-blobstorage-connectionstring`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-agenthub-storagemanager-blobstorage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Agent_ResourceProviderService_Storage_AuthenticationType,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Agent_ResourceProviderService_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-agent-resourceprovider-storage-connectionstring`",
                description: "The connection string to the Azure Storage account used for the agent resource provider.",
                keyVaultSecretName: "foundationallm-agent-resourceprovider-storage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentFactoryAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-agentfactoryapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-agentfactoryapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentFactoryAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentFactoryAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentFactoryAPI_ForceHttpsRedirection,
                minimumVersion: "1.0.0",
                defaultValue: "true",
                description: "By default, the Agent Factory API forces HTTPS redirection. To override this behavior and allow it to handle HTTP requests, set this value to false.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentHubAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-agenthubapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-agenthubapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentHubAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_AgentHubAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_BypassGatekeeper,
                minimumVersion: "1.0.0",
                defaultValue: "false",
                description: "By default, the Core API does not bypass the Gatekeeper API. To override this behavior and allow it to bypass the Gatekeeper API, set this value to true. Beware that bypassing the Gatekeeper means that you bypass content protection and filtering in favor of improved performance. Make sure you understand the risks before setting this value to true.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_DataSourceHubAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-datasourcehubapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-datasourcehubapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_DataSourceHubAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_DataSourceHubAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-gatekeeperapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-gatekeeperapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_Configuration_EnableAzureContentSafety,
                minimumVersion: "1.0.0",
                defaultValue: "true",
                description: "By default, the Gatekeeper API has Azure Content Safety integration enabled. To disable this feature, set this value to false.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_Configuration_EnableMicrosoftPresidio,
                minimumVersion: "1.0.0",
                defaultValue: "true",
                description: "By default, the Gatekeeper API has Microsoft Presidio integration enabled. To disable this feature, set this value to false.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperAPI_ForceHttpsRedirection,
                minimumVersion: "1.0.0",
                defaultValue: "true",
                description: "By default, the Gatekeeper API forces HTTPS redirection. To override this behavior and allow it to handle HTTP requests, set this value to false.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperIntegrationAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-gatekeeperintegrationapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-gatekeeperintegrationapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_GatekeeperIntegrationAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_LangChainAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-langchainapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-langchainapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_LangChainAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_LangChainAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_ManagementAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_ManagementAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Prompt_ResourceProviderService_Storage_AuthenticationType,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Prompt_ResourceProviderService_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-prompt-resourceprovider-storage-connectionstring`",
                description: "The connection string to the Azure Storage account used for the prompt resource provider.",
                keyVaultSecretName: "foundationallm-prompt-resourceprovider-storage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_PromptHubAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-prompthubapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-prompthubapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_PromptHubAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_PromptHubAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_SemanticKernelAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-semantickernelapi-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-apis-semantickernelapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_SemanticKernelAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_SemanticKernelAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-azurecontentsafety-apikey`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-azurecontentsafety-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_HateSeverity,
                minimumVersion: "1.0.0",
                defaultValue: "2",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_SelfHarmSeverity,
                minimumVersion: "1.0.0",
                defaultValue: "2",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_SexualSeverity,
                minimumVersion: "1.0.0",
                defaultValue: "2",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureContentSafety_ViolenceSeverity,
                minimumVersion: "1.0.0",
                defaultValue: "2",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Completions_DeploymentName,
                minimumVersion: "1.0.0",
                defaultValue: "completions",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Completions_MaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "8096",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Completions_ModelName,
                minimumVersion: "1.0.0",
                defaultValue: "gpt-35-turbo",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Completions_ModelVersion,
                minimumVersion: "1.0.0",
                defaultValue: "0301",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Completions_Temperature,
                minimumVersion: "1.0.0",
                defaultValue: "0",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Embeddings_DeploymentName,
                minimumVersion: "1.0.0",
                defaultValue: "embeddings",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Embeddings_MaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "8191",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Embeddings_ModelName,
                minimumVersion: "1.0.0",
                defaultValue: "text-embedding-ada-002",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Embeddings_Temperature,
                minimumVersion: "1.0.0",
                defaultValue: "0",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Endpoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-azureopenai-api-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-azureopenai-api-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_AzureOpenAI_API_Version,
                minimumVersion: "1.0.0",
                defaultValue: "2023-05-15",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_BlobStorageMemorySource_BlobStorageConnection,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-blobstoragememorysource-blobstorageconnection`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-blobstoragememorysource-blobstorageconnection",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_BlobStorageMemorySource_BlobStorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "memory-source",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_BlobStorageMemorySource_ConfigFilePath,
                minimumVersion: "1.0.0",
                defaultValue: "BlobMemorySourceConfig.json",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_AccentColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_AccentTextColor,
                minimumVersion: "1.0.0",
                defaultValue: "#131833",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_BackgroundColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_CompanyName,
                minimumVersion: "1.0.0",
                defaultValue: "FoundationaLLM",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_FavIconUrl,
                minimumVersion: "1.0.0",
                defaultValue: "favicon.ico",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_KioskMode,
                minimumVersion: "1.0.0",
                defaultValue: "false",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_LogoText,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_LogoUrl,
                minimumVersion: "1.0.0",
                defaultValue: "foundationallm-logo-white.svg",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_PageTitle,
                minimumVersion: "1.0.0",
                defaultValue: "FoundationaLLM Chat Copilot",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_PrimaryColor,
                minimumVersion: "1.0.0",
                defaultValue: "#131833",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_PrimaryTextColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_SecondaryColor,
                minimumVersion: "1.0.0",
                defaultValue: "#334581",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_SecondaryTextColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_PrimaryButtonBackgroundColor,
                minimumVersion: "1.0.0",
                defaultValue: "#5472d4",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_PrimaryButtonTextColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_SecondaryButtonBackgroundColor,
                minimumVersion: "1.0.0",
                defaultValue: "#70829a",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Branding_SecondaryButtonTextColor,
                minimumVersion: "1.0.0",
                defaultValue: "#fff",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_CallbackPath,
                minimumVersion: "1.0.0",
                defaultValue: "/signin-oidc",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_ClientId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-chat-entra-clientsecret`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-chat-entra-clientsecret",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_Instance,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes,
                minimumVersion: "1.0.0",
                defaultValue: "api://FoundationaLLM-Auth/Data.Read",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Chat_Entra_TenantId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearch_EndPoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearch_IndexName,
                minimumVersion: "1.0.0",
                defaultValue: "vector-index",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearch_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-cognitivesearch-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-cognitivesearch-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearch_MaxVectorSearchResults,
                minimumVersion: "1.0.0",
                defaultValue: "10",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_BlobStorageConnection,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-cognitivesearchmemorysource-blobstorageconnection`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-cognitivesearchmemorysource-blobstorageconnection",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_BlobStorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "memory-source",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_ConfigFilePath,
                minimumVersion: "1.0.0",
                defaultValue: "ACSMemorySourceConfig.json",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_EndPoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_IndexName,
                minimumVersion: "1.0.0",
                defaultValue: "vector-index",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CognitiveSearchMemorySource_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-cognitivesearchmemorysource-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-cognitivesearchmemorysource-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_CallbackPath,
                minimumVersion: "1.0.0",
                defaultValue: "/signin-oidc",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_ClientId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-coreapi-entra-clientsecret`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-coreapi-entra-clientsecret",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_Instance,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_Scopes,
                minimumVersion: "1.0.0",
                defaultValue: "Data.Read",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreAPI_Entra_TenantId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CoreWorker_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_ChangeFeedLeaseContainer,
                minimumVersion: "1.0.0",
                defaultValue: "leases",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_Containers,
                minimumVersion: "1.0.0",
                defaultValue: "Sessions, UserSessions",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_Database,
                minimumVersion: "1.0.0",
                defaultValue: "database",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_Endpoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-cosmosdb-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-cosmosdb-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_CosmosDB_MonitoredContainers,
                minimumVersion: "1.0.0",
                defaultValue: "Sessions",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_DataSourceHub_DataSourceMetadata_StorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "data-sources",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_DataSourceHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_DataSources_AboutFoundationaLLM_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_DurableSystemPrompt_BlobStorageConnection,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-durablesystemprompt-blobstorageconnection`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-durablesystemprompt-blobstorageconnection",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_DurableSystemPrompt_BlobStorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "system-prompt",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_LangChain_CSVFile_URL,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-langchain-csvfile-url`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-langchain-csvfile-url",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_LangChain_SQLDatabase_TestDB_Password,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-langchain-sqldatabase-testdb-password`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-langchain-sqldatabase-testdb-password",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_LangChain_Summary_MaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "4097",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_LangChain_Summary_ModelName,
                minimumVersion: "1.0.0",
                defaultValue: "gpt-35-turbo",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_LangChainAPI_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-langchainapi-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-langchainapi-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_CallbackPath,
                minimumVersion: "1.0.0",
                defaultValue: "/signin-oidc",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_ClientId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-management-entra-clientsecret`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-management-entra-clientsecret",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_Instance,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_Scopes,
                minimumVersion: "1.0.0",
                defaultValue: "api://FoundationaLLM-Management-Auth/Data.Manage",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Management_Entra_TenantId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_ManagementAPI_Entra_ClientId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_ManagementAPI_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-managementapi-entra-clientsecret`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-managementapi-entra-clientsecret",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_ManagementAPI_Entra_Instance,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_ManagementAPI_Entra_Scopes,
                minimumVersion: "1.0.0",
                defaultValue: "Data.Manage",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_ManagementAPI_Entra_TenantId,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_OpenAI_API_Endpoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_OpenAI_API_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-openai-api-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-openai-api-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_OpenAI_API_Temperature,
                minimumVersion: "1.0.0",
                defaultValue: "0",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_PromptHub_PromptMetadata_StorageContainer,
                minimumVersion: "1.0.0",
                defaultValue: "system-prompt",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_PromptHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-prompthub-storagemanager-blobstorage-connectionstring`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-prompthub-storagemanager-blobstorage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Refinement,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_Key,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-semantickernelapi-openai-key`",
                description: "This is a Key Vault reference.",
                keyVaultSecretName: "foundationallm-semantickernelapi-openai-key",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_ChatCompletionPromptName,
                minimumVersion: "1.0.0",
                defaultValue: "RetailAssistant.Default",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_CompletionsDeployment,
                minimumVersion: "1.0.0",
                defaultValue: "completions",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_CompletionsDeploymentMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "8096",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_EmbeddingsDeployment,
                minimumVersion: "1.0.0",
                defaultValue: "embeddings",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_EmbeddingsDeploymentMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "8191",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_Endpoint,
                minimumVersion: "1.0.0",
                defaultValue: "Enter the URL to the service.",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_CompletionsMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "300",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_CompletionsMinTokens,
                minimumVersion: "1.0.0",
                defaultValue: "50",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_MemoryMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "3000",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_MemoryMinTokens,
                minimumVersion: "1.0.0",
                defaultValue: "1500",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_MessagesMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "3000",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_MessagesMinTokens,
                minimumVersion: "1.0.0",
                defaultValue: "100",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_PromptOptimization_SystemMaxTokens,
                minimumVersion: "1.0.0",
                defaultValue: "1500",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_SemanticKernelAPI_OpenAI_ShortSummaryPromptName,
                minimumVersion: "1.0.0",
                defaultValue: "Summarizer.TwoWords",
                description: "",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationAPI_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The URL of the vectorization API.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationAPI_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-vectorizationapi-apikey`",
                description: "The API key of the vectorization API.",
                keyVaultSecretName: "foundationallm-apis-vectorizationapi-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationAPI_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "The connection string to the Application Insights instance used by the vectorization API.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationWorker_APIUrl,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The URL of the vectorization worker API.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationWorker_APIKey,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-apis-vectorizationworker-apikey`",
                description: "The API key of the vectorization worker API.",
                keyVaultSecretName: "foundationallm-apis-vectorizationworker-apikey",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_APIs_VectorizationWorker_AppInsightsConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-app-insights-connection-string`",
                description: "The connection string to the Application Insights instance used by the vectorization worker API.",
                keyVaultSecretName: "foundationallm-app-insights-connection-string",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_VectorizationWorker,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The settings used by each instance of the vectorization worker service. For more details, see [default vectorization worker settings](../setup-guides/vectorization/vectorization-worker.md#default-vectorization-worker-settings)",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_Queues_Embed_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-queues-connectionstring`",
                description: "The connection string to the Azure Storage account used for the embed vectorization queue.",
                keyVaultSecretName: "foundationallm-vectorization-queues-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_Queues_Extract_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-queues-connectionstring`",
                description: "The connection string to the Azure Storage account used for the extract vectorization queue.",
                keyVaultSecretName: "foundationallm-vectorization-queues-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_Queues_Index_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-queues-connectionstring`",
                description: "The connection string to the Azure Storage account used for the index vectorization queue.",
                keyVaultSecretName: "foundationallm-vectorization-queues-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_Queues_Partition_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-queues-connectionstring`",
                description: "The connection string to the Azure Storage account used for the partition vectorization queue.",
                keyVaultSecretName: "foundationallm-vectorization-queues-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_StateService_Storage_AuthenticationType,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_StateService_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-state-connectionstring`",
                description: "The connection string to the Azure Storage account used for the vectorization state service.",
                keyVaultSecretName: "foundationallm-vectorization-state-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_ResourceProviderService_Storage_AuthenticationType,
                minimumVersion: "1.0.0",
                defaultValue: "",
                description: "The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`.",
                keyVaultSecretName: "",
                contentType: "text/plain",
                sampleObject: null
            ),
            new AppConfigEntry(
                key: Common.Constants.AppConfigurationKeys.FoundationaLLM_Vectorization_ResourceProviderService_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                defaultValue: "Key Vault secret name: `foundationallm-vectorization-resourceprovider-storage-connectionstring`",
                description: "The connection string to the Azure Storage account used for the vectorization state service.",
                keyVaultSecretName: "foundationallm-vectorization-resourceprovider-storage-connectionstring",
                contentType: "text/plain",
                sampleObject: null
            ),
        ];

        /// <summary>
        /// Returns the list of all the app configuration entries for this solution.
        /// </summary>
        /// <returns></returns>
        public static List<AppConfigEntry> GetAppConfigEntries() => Entries;

        /// <summary>
        /// Returns the list of all the app configuration entries for this solution that are required for the given version.
        /// </summary>
        /// <param name="version">The current version of the caller.</param>
        /// <returns></returns>
        public static IEnumerable<AppConfigEntry> GetRequiredConfigsForVersion(string version)
        {
            var currentVersion = new Version(version);
            return Entries.Where(entry => !string.IsNullOrWhiteSpace(entry.MinimumVersion) &&
                                          new Version(entry.MinimumVersion) <= currentVersion);
        }
    }
}
