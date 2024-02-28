# Deployment

## Dependencies

Please install the following dependencies to deploy FLLM.

- [PowerShell 7](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.4):
PowerShell 7 is a cross-platform (Windows, macOS, and Linux) automation tool and scripting language, an evolution of PowerShell that works with the .NET Core framework. It offers enhanced features and performance improvements over its predecessors and is designed for heterogeneous environments and the hybrid cloud. In PowerShell 7, the command-line executable is referred to as pwsh, an alias that is essential for integration with Azure Developer CLI (AZD) hooks and other modern automation scenarios.
- [azd CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd): The Azure Developer CLI (Azure Dev CLI) is a command-line tool designed to streamline the development and deployment of applications on Microsoft's Azure cloud platform. It simplifies various tasks such as setting up development environments, managing resources, and deploying applications by providing a more developer-friendly interface. The Azure Dev CLI aims to enhance productivity by abstracting complex cloud management tasks into simpler, more intuitive commands, making it easier for developers to integrate Azure services into their workflows.
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli): The Azure Command Line Interface (CLI) is a set of commands used to manage Azure resources directly from the command line. It provides a simple and efficient way to automate tasks and manage Azure services, supporting both Windows, macOS, and Linux platforms. Azure CLI is particularly useful for scripting and executing batch operations, offering a comprehensive set of commands that cover almost all aspects of Azure resource management.
- [git](https://git-scm.com/downloads): Git is a distributed version control system designed to handle everything from small to very large projects with speed and efficiency. It allows multiple developers to work on the same codebase simultaneously, tracking and merging changes, and maintaining a complete history of all file revisions. Git is essential for modern software development, supporting branching and merging strategies, and is widely used for its robustness, flexibility, and remote collaboration capabilities.

To develop locally, you will need to install the following dependencies:
- [Docker Desktop](https://www.docker.com/products/docker-desktop/): Docker Desktop is an application for MacOS and Windows machines for the building and sharing of containerized applications and microservices. It provides an integrated environment to use Docker containers, simplifying the process of building, testing, and deploying applications in a consistent and isolated environment. Docker Desktop includes Docker Engine, Docker CLI client, Docker Compose, and other Docker tools, making it a key tool for developers working with container-based applications.

## Deployment Instructions

### Clone the FoundationaLLM repository

```pwsh
git clone https://github.com/solliancenet/foundationallm
```

### Common Configuration
Run the following commands to set the appropriate application registration settings for OIDC authentication.

```text
cd foundationallm/deploy/starter

az login            # Log into Azure CLI
azd auth login      # Log into Azure Developer CLI

# Set your target Subscription and Location
azd env new --location <Supported Azure Region> --subscription <Azure Subscription ID>

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client ID>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

azd env set ENTRA_CORE_API_CLIENT_ID <Core API Client ID>
azd env set ENTRA_CORE_API_SCOPES <Core API Scope>
azd env set ENTRA_CORE_API_TENANT_ID <Core API Tenant ID>

azd env set ENTRA_MANAGEMENT_API_CLIENT_ID <Management API Client ID>
azd env set ENTRA_MANAGEMENT_API_SCOPES <Management API Scope>
azd env set ENTRA_MANAGEMENT_API_TENANT_ID <Management API Tenant ID>

azd env set ENTRA_MANAGEMENT_UI_CLIENT_ID <Management UI Client ID>
azd env set ENTRA_MANAGEMENT_UI_SCOPES <Management UI Scope>
azd env set ENTRA_MANAGEMENT_UI_TENANT_ID <Management UI Tenant ID>

azd env set ENTRA_VECTORIZATION_API_CLIENT_ID <Vectorization API Client ID>
azd env set ENTRA_VECTORIZATION_API_SCOPES <Vectorization API Scope>
azd env set ENTRA_VECTORIZATION_API_TENANT_ID <Vectorization API Tenant ID>

azd env set FOUNDATIONALLM_INSTANCE_ID <guid>
```

>[!NOTE]
> You need to manually generate a GUID for `FOUNDATIONALLM_INSTANCE_ID`.

Bash:

```bash
uuidgen
```

PowerShell:

```powershell
[guid]::NewGuid().ToString()
```

### Bring Your Own Azure OpenAI Instance

If you have an existing Azure OpenAI instance, you can use it by setting the following environment variables:

```text
azd env set OPENAI_NAME <OpenAI Name>
azd env set OPENAI_RESOURCE_GROUP <OpenAI Resource Group>
azd env set OPENAI_SUBSCRIPTION_ID <OpenAI Subscription ID>
```

### Deploy the environment
After configuring the AZD environment above, run `azd up` in the same folder location to provision the infrastructure, update the App Configuration entries, deploy the API and web app services, and import files into the storage account.

```pwsh
azd up
```

# Teardown

To tear down the environment, execute `azd down` in the same folder location.

```pwsh
azd down --purge
```

> Note the `--purge` argument in the command above. This ensures that resources that would otherwise be soft-deleted are instead completely purged from your Azure subscription.
