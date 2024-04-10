# History of breaking changes

## Release 0.6.0

1. Vectorization resource stores use a unique collection name, `Resources`. They also add a new top-level property named `DefaultResourceName`.
2. The items in the `index_references` collection have an property incorrectly named `type` which was renamed to `index_entry_id`.
3. New gateway API, requires the following app configurations:
   - `FoundationaLLM:APIs:GatewayAPI:APIUrl`
   - `FoundationaLLM:APIs:GatewayAPI:APIKey` (with secret `foundationallm-apis-gatewayapi-apikey`)
   - `FoundationaLLM:APIs:GatewayAPI:AppInsightsConnectionString` (with secret `foundationallm-app-insights-connection-string`)
   - `FoundationaLLM:Gateway:AzureOpenAIEndpoints`
4. 