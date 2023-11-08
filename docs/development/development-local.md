# Configure local development environment

- [Configure local development environment](#configure-local-development-environment)
  - [Requirements](#requirements)
  - [UI](#ui)
    - [User Portal](#user-portal)
    - [Chat](#chat)
      - [Chat app settings](#chat-app-settings)
  - [.NET projects](#net-projects)
    - [Core API](#core-api)
      - [Core API app settings](#core-api-app-settings)
    - [Gatekeeper API](#gatekeeper-api)
      - [Gatekeeper API app settings](#gatekeeper-api-app-settings)
    - [Agent Factory API](#agent-factory-api)
      - [Agent Factory API app settings](#agent-factory-api-app-settings)
    - [Semantic Kernel API](#semantic-kernel-api)
      - [Semantic Kernel API app settings](#semantic-kernel-api-app-settings)
  - [Python projects](#python-projects)
    - [Python Environment Variables](#python-environment-variables)
    - [Agent Hub API](#agent-hub-api)
      - [Agent Hub API Environment Variables](#agent-hub-api-environment-variables)
    - [Data Source Hub API](#data-source-hub-api)
      - [Data Source Hub API Environment Variables](#data-source-hub-api-environment-variables)
    - [Prompt Hub API](#prompt-hub-api)
      - [Prompt Hub API Environment Variables](#prompt-hub-api-environment-variables)
    - [LangChain API](#langchain-api)
      - [LangChain API Environment Variables](#langchain-api-environment-variables)

## Requirements

Environment variable needs to be set for Application Configuration Service URL. This environment variable needs to be named `FoundationaLLM:AppConfig:ConnectionString`.

## UI

### User Portal

### Chat

#### Chat app settings

> Make sure the contents of the `appsettings.json` file has this structure and similar values:

```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FoundationaLLM": {
    "AppConfig": {
      "ConnectionString": ""
    }
  }
}
```

> Create the `appsettings.Development.json` file or update it with the following content and replace all `<...>` placeholders with the values from your deployment:

```json
{
  "FoundationaLLM": {
    "APIs": {
      "CoreAPI": {
        "APIUrl": "<...>"
      },
    } 
  }
 }

```

## .NET projects

### Core API

#### Core API app settings

> Make sure the contents of the `appsettings.json` file has this structure and similar values:

```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FoundationaLLM": {
    "AppConfig": {
      "ConnectionString": ""
    }
  }
}
```

> Create the `appsettings.Development.json` file or update it with the following content and replace all `<...>` placeholders with the values from your deployment:

```json
{  
  "APIs": {
    "GatekeeperAPI": {
      "APIUrl": "<...>"
    }
  }
}
```

### Gatekeeper API

#### Gatekeeper API app settings

> Make sure the contents of the `appsettings.json` file has this structure and similar values:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "FoundationaLLM": {
    "AppConfig": {
      "ConnectionString": ""
    }
  }
}
```

> Create the `appsettings.Development.json` file or update it with the following content and replace all `<...>` placeholders with the values from your deployment:

```json
{
  "FoundationaLLM": {
    "APIs": {
      "AgentFactoryAPI": {
        "APIUrl": "<...>"
      },
    
      "GatekeeperAPI": {
        "APIUrl": "<...>"
      }
    }
  }
}
```

### Agent Factory API

#### Agent Factory API app settings

> Make sure the contents of the `appsettings.json` file has this structure and similar values:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "FoundationaLLM": {
    "AppConfig": {
      "ConnectionString": ""
    }
  }
}
```

> Create the `appsettings.Development.json` file or update it with the following content and replace all `<...>` placeholders with the values from your deployment:

```json
{
  "FoundationaLLM": {
    "APIs": {
      "AgentFactoryAPI": {
        "APIUrl": "<...>"
      },
      "LangChainAPI": {
        "APIUrl": "<...>"
      },
      "SemanticKernelAPI": {
        "APIUrl": "<...>"
      },
      "AgentHubAPI": {
        "APIUrl": "<...>"
      },
      "PromptHubAPI": {
        "APIUrl": "<...>"
      },
      "DataSourceHubAPI": {
        "APIUrl": "<...>"
      }
    }
  }
}
```

### Semantic Kernel API

#### Semantic Kernel API app settings

> Make sure the contents of the `appsettings.json` file has this structure and similar values:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.SemanticKernel": "Error"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.SemanticKernel": "Error"
      }
    }
  },
  "AllowedHosts": "*",
  "FoundationaLLM": {
    "CognitiveSearch": {
      "IndexName": "vector-index",
      "MaxVectorSearchResults": 10
    },
    "OpenAI": {
      "CompletionsDeployment": "completions",
      "CompletionsDeploymentMaxTokens": 8096,
      "EmbeddingsDeployment": "embeddings",
      "EmbeddingsDeploymentMaxTokens": 8191,
      "ChatCompletionPromptName": "RetailAssistant.Default",
      "ShortSummaryPromptName": "Summarizer.TwoWords",
      "PromptOptimization": {
        "CompletionsMinTokens": 50,
        "CompletionsMaxTokens": 300,
        "SystemMaxTokens": 1500,
        "MemoryMinTokens": 1500,
        "MemoryMaxTokens": 3000,
        "MessagesMinTokens": 100,
        "MessagesMaxTokens": 3000
      }
    },
    "DurableSystemPrompt": {
      "BlobStorageContainer": "prompts"
    },
    "CognitiveSearchMemorySource": {
      "IndexName": "vector-index",
      "ConfigBlobStorageContainer": "memory-source",
      "ConfigFilePath": "ACSMemorySourceConfig.json"
    },
    "BlobStorageMemorySource": {
      "ConfigBlobStorageContainer": "memory-source",
      "ConfigFilePath": "BlobMemorySourceConfig.json"
    },
    "SemanticKernelOrchestration": {
      "APIKeySecretName": "foundationallm-semantickernel-api-key"
    }
  }
}
```

> Create the `appsettings.Development.json` file or update it with the following content and replace all `<...>` placeholders with the values from your deployment:

```json
{
  "FoundationaLLM": {
    "CognitiveSearch": {
      "Endpoint": "https://<...>-cog-search.search.windows.net",
      "Key": "<...>"
    },
    "OpenAI": {
      "Endpoint": "https://<...>-openai.openai.azure.com/",
      "Key": "<...>"
    },
    "DurableSystemPrompt": {
      "BlobStorageConnection": "<...>"
    },
    "CognitiveSearchMemorySource": {
      "Endpoint": "https://<...>-cog-search.search.windows.net",
      "Key": "<...>",
      "ConfigBlobStorageConnection": "<...>"
    },
    "BlobStorageMemorySource": {
      "ConfigBlobStorageConnection": "<...>"
    }
  }
}
```

## Python projects

### Python Environment Variables

Create a local environment variable named `foundationallm-app-configuration-uri`. The value should be the URI of the Azure App Configuration service and _not_ the connection string. We use role-based access controls (RBAC) to access the Azure App Configuration service, so the connection string is not required.

| Name | Value | Description |
| ---- | ----- | ----------- |
| foundationallm-app-configuration-uri | REDACTED | Azure App Configuration URI |

### Agent Hub API

#### Agent Hub API Environment Variables

| Name | Value | Description |
| ---- | ----- | ----------- |

### Data Source Hub API

#### Data Source Hub API Environment Variables

| Name | Value | Description |
| ---- | ----- | ----------- |

### Prompt Hub API

#### Prompt Hub API Environment Variables

| Name | Value | Description |
| ---- | ----- | ----------- |

### LangChain API

#### LangChain API Environment Variables

| Name | Value | Description |
| ---- | ----- | ----------- |
