# History of breaking changes

> [!NOTE]
> This section is for changes that are not yet released but will affect future releases.

## Breaking changes that will affect future releases.

1. Vectorization resource stores use a unique collection name, `Resources`. They also add a new top-level property named `DefaultResourceName`.
2. The items in the `index_references` collection have an property incorrectly named `type` which was renamed to `index_entry_id`.
3. New gateway API, requires the following app configurations:
   - `FoundationaLLM:APIs:GatewayAPI:APIUrl`
   - `FoundationaLLM:APIs:GatewayAPI:APIKey` (with secret `foundationallm-apis-gatewayapi-apikey`)
   - `FoundationaLLM:APIs:GatewayAPI:AppInsightsConnectionString` (with secret `foundationallm-app-insights-connection-string`)
   - `FoundationaLLM:Gateway:AzureOpenAIAccounts`
4. The `AgentFactory` and `AgentFactoryAPI` classes have been renamed to `Orchestration` and `OrchestrationAPI`, respectively. The following App Config settings need to be replaced in existing environments:

    - `FoundationaLLM:APIs:AgentFactoryAPI:APIKey` -> `FoundationaLLM:APIs:OrchestrationAPI:APIKey`
    - `FoundationaLLM:APIs:AgentFactoryAPI:APIUrl` -> `FoundationaLLM:APIs:OrchestrationAPI:APIUrl`
    - `FoundationaLLM:APIs:AgentFactoryAPI:AppInsightsConnectionString` -> `FoundationaLLM:APIs:OrchestrationAPI:AppInsightsConnectionString`
    - `FoundationaLLM:Events:AzureEventGridEventService:Profiles:AgentFactoryAPI` -> `FoundationaLLM:Events:AzureEventGridEventService:Profiles:OrchestrationAPI`
    - `FoundationaLLM:APIs:AgentFactoryAPI:ForceHttpsRedirection` -? `FoundationaLLM:APIs:OrchestrationAPI:ForceHttpsRedirection`

5. The following Key Vault secrets need to be replaced in existing environments:

    - `foundationallm-apis-agentfactoryapi-apikey` -> `foundationallm-apis-orchestrationapi-apikey`

\* There is an upgrade script available that migrates these settings and secrets to their new names.

6. The following App Config settings are no longer needed:
   
    - `FoundationaLLM:Vectorization:Queues:Embed:ConnectionString`
    - `FoundationaLLM:Vectorization:Queues:Extract:ConnectionString`
    - `FoundationaLLM:Vectorization:Queues:Index:ConnectionString`
    - `FoundationaLLM:Vectorization:Queues:Partition:ConnectionString`

7. The following Key Vault secret is no longer needed:

    - `foundationallm-vectorization-queues-connectionstring`

8. The following App Config settings need to be added as key-values:
   
   - `FoundationaLLM:Vectorization:Queues:Embed:AccountName` (set to the name of the storage account that contains the vectorization queues - e.g., `stejahszxcubrpi`)
   - `FoundationaLLM:Vectorization:Queues:Extract:AccountName` (set to the name of the storage account that contains the vectorization queues - e.g., `stejahszxcubrpi`)
   - `FoundationaLLM:Vectorization:Queues:Index:AccountName` (set to the name of the storage account that contains the vectorization queues - e.g., `stejahszxcubrpi`)
   - `FoundationaLLM:Vectorization:Queues:Partition:AccountName` (set to the name of the storage account that contains the vectorization queues - e.g., `stejahszxcubrpi`)