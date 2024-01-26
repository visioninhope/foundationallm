# Directly calling the APIs

Typically, the only interaction with the Foundationa**LLM** (FLLM) APIs is indirectly through the User Portal and Management Portal. However, you can also call the APIs directly to perform certain tasks, such as using your [configured FLLM agents](../setup-guides/agents/index.md) to perform completions (via the Core API), or updating your branding configurations (via the Management API).

## API architecture

The FLLM architecture contains layers of APIs that are used to perform different tasks along a call chain, starting with the **Core API**. The following diagram shows the API architecture:

```mermaid
graph TD;
    A[CoreAPI] -->|1. User Request| B[GatekeeperAPI] -->|Gatekeeper Extensions| BB[GatekeeperIntegrationAPI]
    A -...->|"1a. User Request (Bypass Gatekeeper)"| C[AgentFactoryAPI]
    B ---->|2. Processed Request| C[AgentFactoryAPI]
    C -->|3. Request| E[(AgentHubAPI)]
    C --->|4. Instantiate Agent| D[[Agent]]
    D -->|Request| F[(PromptHubAPI)]
    D -->|Request| G[(DataSourceHubAPI)]
    E -->|Metadata| C
    F -->|Metadata| D
    G -->|Metadata| D
    D -->|Hydrated Agent| C
    D -->|5. Composed Information| H[OrchestrationWrapperAPI]
    H -->|6. Response| D
    C -->|7. Response| B
    C -...->|"7a. Response (Bypass Gatekeeper)"| A
    B -->|8. Final Response| A

```

> [!NOTE]
> Notice that there is an alternate path that bypasses the Gatekeeper API. This path is used when the `FoundationaLLM:APIs:CoreAPI:BypassGatekeeper` configuration value is set to `true`. By default, the Core API does not bypass the Gatekeeper API. Beware that bypassing the Gatekeeper means that you bypass content protection and filtering in favor of improved performance. Make sure you understand the risks before setting this value to `true`.


