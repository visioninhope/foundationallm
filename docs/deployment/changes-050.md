# Changes to the 0.5.0 release
This document outlines the changes made to the FoundationaLLM project in the 0.5.0 release.

## AzureAIDirect and AzureOpenAIDirect Orchestration
So far the FoundationaLLM platform offered orchestration through its solid infrastructure for `Knowledge Management Agents` and `internal Context Agents`, but some customers want to take control of their own orchestration. To address this need, we have introduced two new orchestrators: `AzureAIDirect` and `AzureOpenAIDirect`. These orchestrators allow customers to use their own AI models for orchestration while staying within the FoundationaLLM platform.

## AzureAIDirect
This method in FoundationaLLM will allow customers to create their agent by creating an Azure Machine Learning Workspace in their subscription where they can deploy AI models like Llama-2, Mistral and others. The agent will then pass the prompt directly to the LLM and get the response back.  The value of this method is that the customer can use their own AI models for orchestration while staying within the FoundationaLLM platform.

## AzureOpenAIDirect
This method in FoundationaLLM will allow customers to create their agent by creating an Azure Machine Learning Workspace in their subscription where they can deploy OpenAI models like gpt-3.5-turbo, gpt-4 and others. The agent will then pass the prompt directly to the LLM and get the response back.  The value of this method is that the customer can use their own OpenAI models for orchestration while staying within the FoundationaLLM platform.

## Override LLM parameters in completion requests
In the 0.5.0 release, we have added the ability to override the LLM parameters in completion requests. This feature allows customers to pass the parameters to the LLM model directly from the agent. This feature is useful when the customer wants to change the parameters for a specific request.

```json
 "model_parameters": {
      "temperature": 0.8,
      "max_new_tokens": 1000,
      "deployment_name": "llama-2-7b-chat-19"
    }
```

## RBAC Roles

RBAC roles `reader, Contributor and User Access Administrator` are now activated on:
- Management API
- Core API
- Agent Factory API
 
>[!NOTE]
>Upgrading from 0.4.0 to 0.5.0 requires that these are configured for the APIs listed above for system to function correctly.

## Vectorization Changes

- We have improved validation of Vectorization requests so if a type of file is not supported by the system, the system will return an error message immediately.
- After a configured amount of attempts to a vectorization request, the system will stop processing given steps.
- The system now uses a Dynamic pace of proccessing in Vectorization workers. If the system is idle for more than 



