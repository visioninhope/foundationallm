using FoundationaLLM.Common.Models.Configuration.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Configuration.Catalog
{
    /// <summary>
    /// A catalog of Key Vault secrets used in this solution.
    /// </summary>
    public static class KeyVaultSecretsCatalog
    {
        /// <summary>
        /// The list of Key Vault secret entries.
        /// </summary>
        public static readonly List<KeyVaultSecretEntry> Entries =
        [
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_AgentHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_Agent_ResourceProvider_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                description: "The connection string to the Azure Storage account used for the agent resource provider."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_AgentFactoryAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_AgentHubAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_DataSourceHubAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_GatekeeperAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_GatekeeperIntegrationAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_LangChainAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_Prompt_ResourceProvider_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                description: "The connection string to the Azure Storage account used for the prompt resource provider."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_PromptHubAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_SemanticKernelAPI_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_AzureContentSafety_APIKey,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_AzureOpenAI_Api_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_BlobStorageMemorySource_Blobstorageconnection,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Chat_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_CognitiveSearch_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_CognitiveSearchMemorySource_Blobstorageconnection,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_CognitiveSearchMemorySource_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Coreapi_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_CosmosDB_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_DataSourceHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_DataSourceHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_DurableSystemPrompt_BlobStorageConnection,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_LangChain_CsvFile_Url,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_LangChain_SQLDatabase_Testdb_Password,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_LangChainAPI_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Management_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Managementapi_Entra_ClientSecret,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_OpenAI_Api_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_PromptHub_StorageManager_BlobStorage_ConnectionString,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_SemanticKernelAPI_OpenAI_Key,
                minimumVersion: "1.0.0",
                description: ""
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_VectorizationAPI_APIKey,
                minimumVersion: "1.0.0",
                description: "The API key of the vectorization API."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description: "The connection string to the Application Insights instance used by the vectorization API."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_APIs_VectorizationWorker_APIKey,
                minimumVersion: "1.0.0",
                description: "The API key of the vectorization worker API."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_App_Insights_Connection_String,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Application Insights instance used by the vectorization worker API."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Vectorization_Queues_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the embed vectorization queue."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Vectorization_Queues_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the extract vectorization queue."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Vectorization_Queues_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the index vectorization queue."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Vectorization_Queues_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the partition vectorization queue."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames.FoundationaLLM_Vectorization_State_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the vectorization state service."
            ),
            new(
                secretName: Common.Constants.KeyVaultSecretNames
                    .FoundationaLLM_Vectorization_ResourceProvider_Storage_ConnectionString,
                minimumVersion: "1.0.0",
                description:
                "The connection string to the Azure Storage account used for the vectorization state service."
            )
        ];
    }

}
