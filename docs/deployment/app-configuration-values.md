# Azure App Configuration values

FoundationaLLM uses Azure App Configuration to store configuration values, Key Vault secret references, and feature flags. Doing so helps reduce duplication and provides a convenient way to manage these settings in one place. It also allows you to change the settings without having to redeploy the solution. Since several settings can be shared by multiple projects, we do not specify the project name in the configuration key names.

## Configuration values

### Hubs

#### Agent Hub

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:AgentHub:AgentMetadata:StorageContainer` | agents |   |
| `FoundationaLLM:AgentHub:StorageManager:BlobStorage:ConnectionString` | Key Vault secret name: `foundationallm-agenthub-storagemanager-blobstorage-connectionstring` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:AgentHubAPI:APIKey` | Key Vault secret name: `foundationallm-apis-agenthubapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:AgentHubAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:AgentHubAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |

#### Prompt Hub

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:PromptHubAPI:APIKey` | Key Vault secret name: `foundationallm-apis-prompthubapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:PromptHubAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:PromptHubAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:PromptHub:PromptMetadata:StorageContainer` | system-prompt |   |
| `FoundationaLLM:PromptHub:StorageManager:BlobStorage:ConnectionString` | Key Vault secret name: `foundationallm-prompthub-storagemanager-blobstorage-connectionstring` | This is a Key Vault reference. |

#### Data Source Hub

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:DataSourceHubAPI:APIKey` | Key Vault secret name: `foundationallm-apis-datasourcehubapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:DataSourceHubAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:DataSourceHubAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:DataSourceHub:DataSourceMetadata:StorageContainer` | data-sources |   |
| `FoundationaLLM:DataSourceHub:StorageManager:BlobStorage:ConnectionString` | Key Vault secret name: `foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring` | This is a Key Vault reference. |

### Agent Factory API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:AgentFactoryAPI:APIKey` | Key Vault secret name: `foundationallm-apis-agentfactoryapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:AgentFactoryAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:AgentFactoryAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:AgentFactoryAPI:ForceHttpsRedirection` | true | By default, the Agent Factory API forces HTTPS redirection. To override this behavior and allow it to handle HTTP requests, set this value to false. |

### Core API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:CoreAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:CoreAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:CoreAPI:BypassGatekeeper` | `false` | Boolean (`true`/`false`) expressing whether or not to bypass the Gatekeeper API, which offers content safety and data anonymization functionality, for optimal performance. |
| `FoundationaLLM:CoreAPI:Entra:CallbackPath` | /signin-oidc |   |
| `FoundationaLLM:CoreAPI:Entra:ClientId` |   |   |
| `FoundationaLLM:CoreAPI:Entra:ClientSecret` | Key Vault secret name: `foundationallm-coreapi-entra-clientsecret` | This is a Key Vault reference. |
| `FoundationaLLM:CoreAPI:Entra:Instance` | Enter the URL to the service. |   |
| `FoundationaLLM:CoreAPI:Entra:Scopes` | Data.Read |   |
| `FoundationaLLM:CoreAPI:Entra:TenantId` |   |   |

### Gatekeeper API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:GatekeeperAPI:APIKey` | Key Vault secret name: `foundationallm-apis-gatekeeperapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:GatekeeperAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:GatekeeperAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:GatekeeperAPI:Configuration:EnableAzureContentSafety` | true | By default, the Gatekeeper API has Azure Content Safety integration enabled. To disable this feature, set this value to false. |
| `FoundationaLLM:APIs:GatekeeperAPI:Configuration:EnableMicrosoftPresidio` | true | By default, the Gatekeeper API has Microsoft Presidio integration enabled. To disable this feature, set this value to false. |
| `FoundationaLLM:APIs:GatekeeperAPI:ForceHttpsRedirection` | true | By default, the Gatekeeper API forces HTTPS redirection. To override this behavior and allow it to handle HTTP requests, set this value to false. |
| `FoundationaLLM:Refinement` |   |   |

### Gatekeeper Integration API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:GatekeeperIntegrationAPI:APIKey` | Key Vault secret name: `foundationallm-apis-gatekeeperintegrationapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:GatekeeperIntegrationAPI:APIUrl` | Enter the URL to the service. |   |

### LangChain API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:LangChainAPI:APIKey` | Key Vault secret name: `foundationallm-apis-langchainapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:LangChainAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:LangChainAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:LangChain:Summary:MaxTokens` | 4097 |   |
| `FoundationaLLM:LangChain:Summary:ModelName` | gpt-35-turbo |   |
| `FoundationaLLM:LangChainAPI:Key` | Key Vault secret name: `foundationallm-langchainapi-key` | This is a Key Vault reference. |

### Semantic Kernel API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:SemanticKernelAPI:APIKey` | Key Vault secret name: `foundationallm-apis-semantickernelapi-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:APIs:SemanticKernelAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:SemanticKernelAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:SemanticKernelAPI:OpenAI:Key` | Key Vault secret name: `foundationallm-semantickernelapi-openai-key` | This is a Key Vault reference. |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.ChatCompletionPromptName` | RetailAssistant.Default |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.CompletionsDeployment` | completions |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.CompletionsDeploymentMaxTokens` | 8096 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.EmbeddingsDeployment` | embeddings |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.EmbeddingsDeploymentMaxTokens` | 8191 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.Endpoint` | Enter the URL to the service. |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.CompletionsMaxTokens` | 300 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.CompletionsMinTokens` | 50 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.MemoryMaxTokens` | 3000 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.MemoryMinTokens` | 1500 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.MessagesMaxTokens` | 3000 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.MessagesMinTokens` | 100 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.PromptOptimization.SystemMaxTokens` | 1500 |   |
| `FoundationaLLM:SemanticKernelAPI:OpenAI.ShortSummaryPromptName` | Summarizer.TwoWords |   |

### Management API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:ManagementAPI:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:APIs:ManagementAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:ManagementAPI:Entra:ClientId` |   |   |
| `FoundationaLLM:ManagementAPI:Entra:ClientSecret` | Key Vault secret name: `foundationallm-managementapi-entra-clientsecret` | This is a Key Vault reference. |
| `FoundationaLLM:ManagementAPI:Entra:Instance` | Enter the URL to the service. |   |
| `FoundationaLLM:ManagementAPI:Entra:Scopes` | Data.Manage |   |
| `FoundationaLLM:ManagementAPI:Entra:TenantId` |   |   |
| `FoundationaLLM:Instance:Id` | | Deployment GUID populated by the deployment scripts. |

### Azure Content Safety

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:AzureContentSafety:APIKey` | Key Vault secret name: `foundationallm-azurecontentsafety-apikey` | This is a Key Vault reference. |
| `FoundationaLLM:AzureContentSafety:APIUrl` | Enter the URL to the service. |   |
| `FoundationaLLM:AzureContentSafety:HateSeverity` | 2 |   |
| `FoundationaLLM:AzureContentSafety:SelfHarmSeverity` | 2 |   |
| `FoundationaLLM:AzureContentSafety:SexualSeverity` | 2 |   |
| `FoundationaLLM:AzureContentSafety:ViolenceSeverity` | 2 |   |

### Azure OpenAI

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:AzureOpenAI:API:Completions:DeploymentName` | completions |   |
| `FoundationaLLM:AzureOpenAI:API:Completions:MaxTokens` | 8096 |   |
| `FoundationaLLM:AzureOpenAI:API:Completions:ModelName` | gpt-35-turbo |   |
| `FoundationaLLM:AzureOpenAI:API:Completions:ModelVersion` | 0301 |   |
| `FoundationaLLM:AzureOpenAI:API:Completions:Temperature` | 0 |   |
| `FoundationaLLM:AzureOpenAI:API:Embeddings:DeploymentName` | embeddings |   |
| `FoundationaLLM:AzureOpenAI:API:Embeddings:MaxTokens` | 8191 |   |
| `FoundationaLLM:AzureOpenAI:API:Embeddings:ModelName` | text-embedding-ada-002 |   |
| `FoundationaLLM:AzureOpenAI:API:Embeddings:Temperature` | 0 |   |
| `FoundationaLLM:AzureOpenAI:API:Endpoint` | Enter the URL to the service. |   |
| `FoundationaLLM:AzureOpenAI:API:Key` | Key Vault secret name: `foundationallm-azureopenai-api-key` | This is a Key Vault reference. |
| `FoundationaLLM:AzureOpenAI:API:Version` | 2023-05-15 |   |

### OpenAI

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:OpenAI:API:Endpoint` | Enter the URL to the service. |   |
| `FoundationaLLM:OpenAI:API:Key` | Key Vault secret name: `foundationallm-openai-api-key` | This is a Key Vault reference. |
| `FoundationaLLM:OpenAI:API:Temperature` | 0 |   |

### Azure Blob Storage

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:BlobStorageMemorySource:BlobStorageConnection` | Key Vault secret name: `foundationallm-blobstoragememorysource-blobstorageconnection` | This is a Key Vault reference. |
| `FoundationaLLM:BlobStorageMemorySource:BlobStorageContainer` | memory-source |   |
| `FoundationaLLM:BlobStorageMemorySource:ConfigFilePath` | BlobMemorySourceConfig.json |   |

### Branding

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:Branding:AccentColor` | #fff |   |
| `FoundationaLLM:Branding:AccentTextColor` | #131833 |   |
| `FoundationaLLM:Branding:AllowAgentSelection` | default, SDZWA | These are merely sample agent names. Define one or more agents configured for your environment. **Note:** This value corresponds with the `FoundationaLLM-AllowAgentHint` feature flag. If the feature flag is `true`, then the User Portal UI uses these values to provide agent hints to the Agent Hub in completions-based requests. Otherwise, these values are ignored. |
| `FoundationaLLM:Branding:BackgroundColor` | #fff |   |
| `FoundationaLLM:Branding:CompanyName` | FoundationaLLM |   |
| `FoundationaLLM:Branding:FavIconUrl` | favicon.ico |   |
| `FoundationaLLM:Branding:KioskMode` | false |   |
| `FoundationaLLM:Branding:LogoText` |   |   |
| `FoundationaLLM:Branding:LogoUrl` | foundationallm-logo-white.svg |   |
| `FoundationaLLM:Branding:PageTitle` | FoundationaLLM Chat Copilot |   |
| `FoundationaLLM:Branding:PrimaryColor` | #131833 |   |
| `FoundationaLLM:Branding:PrimaryTextColor` | #fff |   |
| `FoundationaLLM:Branding:SecondaryColor` | #334581 |   |
| `FoundationaLLM:Branding:SecondaryTextColor` | #fff |   |
| `FoundationaLLM:Branding:PrimaryButtonBackgroundColor` | #5472d4 |   |
| `FoundationaLLM:Branding:PrimaryButtonTextColor` | #fff |   |
| `FoundationaLLM:Branding:SecondaryButtonBackgroundColor` | #70829a |   |
| `FoundationaLLM:Branding:SecondaryButtonTextColor` | #fff |   |

### Chat UI

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:Chat:Entra:CallbackPath` | /signin-oidc |   |
| `FoundationaLLM:Chat:Entra:ClientId` |   |   |
| `FoundationaLLM:Chat:Entra:ClientSecret` | Key Vault secret name: `foundationallm-chat-entra-clientsecret` | This is a Key Vault reference. |
| `FoundationaLLM:Chat:Entra:Instance` | Enter the URL to the service. |   |
| `FoundationaLLM:Chat:Entra:Scopes` | api://FoundationaLLM-Auth/Data.Read |   |
| `FoundationaLLM:Chat:Entra:TenantId` |   |   |

### Cognitive Search

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:CognitiveSearch:EndPoint` | Enter the URL to the service. |   |
| `FoundationaLLM:CognitiveSearch:IndexName` | vector-index |   |
| `FoundationaLLM:CognitiveSearch:Key` | Key Vault secret name: `foundationallm-cognitivesearch-key` | This is a Key Vault reference. |
| `FoundationaLLM:CognitiveSearch:MaxVectorSearchResults` | 10 |   |
| `FoundationaLLM:CognitiveSearchMemorySource:BlobStorageConnection` | Key Vault secret name: `foundationallm-cognitivesearchmemorysource-blobstorageconnection` | This is a Key Vault reference. |
| `FoundationaLLM:CognitiveSearchMemorySource:BlobStorageContainer` | memory-source |   |
| `FoundationaLLM:CognitiveSearchMemorySource:ConfigFilePath` | ACSMemorySourceConfig.json |   |
| `FoundationaLLM:CognitiveSearchMemorySource:EndPoint` | Enter the URL to the service. |   |
| `FoundationaLLM:CognitiveSearchMemorySource:IndexName` | vector-index |   |
| `FoundationaLLM:CognitiveSearchMemorySource:Key` | Key Vault secret name: `foundationallm-cognitivesearchmemorysource-key` | This is a Key Vault reference. |

### Core Worker

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:CoreWorker:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | This is a Key Vault reference. |
| `FoundationaLLM:CosmosDB:ChangeFeedLeaseContainer` | leases |   |
| `FoundationaLLM:CosmosDB:Containers` | Sessions, UserSessions |   |
| `FoundationaLLM:CosmosDB:Database` | database |   |
| `FoundationaLLM:CosmosDB:Endpoint` | Enter the URL to the service. |   |
| `FoundationaLLM:CosmosDB:Key` | Key Vault secret name: `foundationallm-cosmosdb-key` | This is a Key Vault reference. |
| `FoundationaLLM:CosmosDB:MonitoredContainers` | Sessions |   |

### Agent Data Sources

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:DataSources:AboutFoundationaLLM:BlobStorage:ConnectionString` | Key Vault secret name: `foundationallm-datasourcehub-storagemanager-blobstorage-connectionstring` | This is a Key Vault reference. |
| `FoundationaLLM:DurableSystemPrompt:BlobStorageConnection` | Key Vault secret name: `foundationallm-durablesystemprompt-blobstorageconnection` | This is a Key Vault reference. |
| `FoundationaLLM:DurableSystemPrompt:BlobStorageContainer` | system-prompt |   |
| `FoundationaLLM:LangChain:CSVFile:URL` | Key Vault secret name: `foundationallm-langchain-csvfile-url` | This is a Key Vault reference. |
| `FoundationaLLM:LangChain:SQLDatabase:TestDB:Password` | Key Vault secret name: `foundationallm-langchain-sqldatabase-testdb-password` | This is a Key Vault reference. |

### Management UI

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:Management:Entra:CallbackPath` | /signin-oidc |   |
| `FoundationaLLM:Management:Entra:ClientId` |   |   |
| `FoundationaLLM:Management:Entra:ClientSecret` | Key Vault secret name: `foundationallm-management-entra-clientsecret` | This is a Key Vault reference. |
| `FoundationaLLM:Management:Entra:Instance` | Enter the URL to the service. |   |
| `FoundationaLLM:Management:Entra:Scopes` | api://FoundationaLLM-Management-Auth/Data.Manage |   |
| `FoundationaLLM:Management:Entra:TenantId` |   |   |

### Vectorization

#### Vectorization API

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:VectorizationAPI:APIUrl` | | The URL of the vectorization API. |
| `FoundationaLLM:APIs:VectorizationAPI:APIKey` | Key Vault secret name: `foundationallm-apis-vectorizationapi-apikey` | The API key of the vectorization API. |
| `FoundationaLLM:APIs:VectorizationAPI:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | The connection string to the Application Insights instance used by the vectorization API. |

#### Vectorization Worker

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM:APIs:VectorizationWorker:APIUrl` | | The URL of the vectorization worker API. |
| `FoundationaLLM:APIs:VectorizationWorker:APIKey` | Key Vault secret name: `foundationallm-apis-vectorizationworker-apikey` | The API key of the vectorization worker API. |
| `FoundationaLLM:APIs:VectorizationWorker:AppInsightsConnectionString` | Key Vault secret name: `foundationallm-app-insights-connection-string` | The connection string to the Application Insights instance used by the vectorization worker API. |
| `FoundationaLLM:Vectorization:VectorizationWorker` | | The settings used by each instance of the vectorization worker service. For more details, see [default vectorization worker settings](../setup-guides/vectorization/vectorization-worker.md#default-vectorization-worker-settings) |
| `FoundationaLLM:Vectorization:Queues:Embed:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-queues-connectionstring` | The connection string to the Azure Storage account used for the embed vectorization queue. |
| `FoundationaLLM:Vectorization:Queues:Extract:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-queues-connectionstring` | The connection string to the Azure Storage account used for the extract vectorization queue. |
| `FoundationaLLM:Vectorization:Queues:Index:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-queues-connectionstring` | The connection string to the Azure Storage account used for the index vectorization queue. |
| `FoundationaLLM:Vectorization:Queues:Partition:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-queues-connectionstring` | The connection string to the Azure Storage account used for the partition vectorization queue. |
| `FoundationaLLM:Vectorization:StateService:Storage:AuthenticationType` | | The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`. |
| `FoundationaLLM:Vectorization:StateService:Storage:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-state-connectionstring` | The connection string to the Azure Storage account used for the vectorization state service. |
| `FoundationaLLM:Vectorization:ResourceProviderService:Storage:AuthenticationType` | | The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`. |
| `FoundationaLLM:Vectorization:ResourceProviderService:Storage:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-resourceprovider-storage-connectionstring` | The connection string to the Azure Storage account used for the vectorization state service. |
| `FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:APIKey` | Key Vault secret name: `foundationallm-vectorization-semantickerneltextembedding-openai-apikey` | The API key used to connect to the Azure OpenAI service.
| `FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:AuthenticationType` | | The authentication type used to connect to the Azure OpenAI service. Can be one of `AzureIdentity` or `APIKey`.
| `FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:DeploymentName` | | The name of the Azure OpenAI model deployment. The default value is `embeddings`.
| `FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:Endpoint` | | The endpoint of the Azure OpenAI service.
| `FoundationaLLM:Vectorization:AzureAISearchIndexingService:APIKey` | Key Vault secret name: `foundationallm-vectorization-azureaisearch-apikey` | The API key used to connect to the Azure OpenAI service.
| `FoundationaLLM:Vectorization:AzureAISearchIndexingService:AuthenticationType` | | The authentication type used to connect to the Azure OpenAI service. Can be one of `AzureIdentity` or `APIKey`.
| `FoundationaLLM:Vectorization:AzureAISearchIndexingService:Endpoint` | | The endpoint of the Azure OpenAI service.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:AuthenticationType` | | The authentication type used to connect to the underlying storage. Can be one of `AzureIdentity`, `AccountKey`, or `ConnectionString`.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:ConnectionString` | Key Vault secret name: `foundationallm-vectorization-contentsources-<NAME>-connectionstring` | The connection string to the Azure Storage account used for the the Azure Data Lake vectorization content source.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:ClientId` | | The Application (client) Id of the Microsoft Entra ID App Registration. For more details, see the [Entra ID App Registration](#entra-id-app-registration) section.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:TenantId` | | The unique identifier of the SharePoint Online tenant.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:KeyVaultURL` | | The URL of the KeyVault instance (NOT secret) where the X.509 Certificate is stored. The URL should take the form `[VAULT NAME].vault.azure.net`.
|
`FoundationaLLM:Vectorization:ContentSources:<NAME>:CertificateName` | | The name of the X.509 Certificate.

## Feature flags

| Key | Default Value | Description |
| --- | ------------- | ----------- |
| `FoundationaLLM-AllowAgentHint` | `false` | Used for demo purposes. If the feature is enabled, the User Portal UI displays an agent hint selector for a chat session and sends an `X-AGENT-HINT` header with the selected agent name (if applicable) to all HTTP requests to the Core API. This header flows downstream to the Agent Hub, forcing the resolver to use the specified agent. The Agent Hub only uses this header value if this feature flag is enabled, as an added protective measure. |
