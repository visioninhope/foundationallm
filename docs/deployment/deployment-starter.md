# Deployment - Starter

Foundationa**LLM** deploys into your own Azure Subscription. By default it will deploy to Azure Container Apps (ACA) that make it fast to get started. When you want to deploy to production at scale, you can also deploy to Azure Kubernetes Service (AKS). Given that there are Azure Subscription quota limits to the number of Azure OpenAI Service resources you can deploy, you can choose to use an existing Azure OpenAI Service resource instead of a creating a new one with your deployment.

## Prerequisites

- Azure Subscription (Subscription needs to be whitelisted for Azure OpenAI).
- Subscription access to Azure OpenAI service. Start here to [Request Access to Azure OpenAI Service](https://customervoice.microsoft.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR7en2Ais5pxKtso_Pz4b1_xUNTZBNzRKNlVQSFhZMU9aV09EVzYxWFdORCQlQCN0PWcu).
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- Docker Desktop.
- Azure CLI ([v2.51.0 or greater](https://docs.microsoft.com/cli/azure/install-azure-cli)).
- Azure Developer CLI ([v1.6.1 or greater](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd))
- PowerShell 7 ([7.4.1 or greater](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.4))
    > Note: Powershell 7 is required for `pwsh` to work in the AZD hooks as an alias to `powershell`
- Visual Studio 2022 (only needed if you plan to run/debug the solution locally).
- Minimum quota of 65 CPUs across all VM family types. Start here to [Manage VM Quotas](https://learn.microsoft.com/azure/quotas/per-vm-quota-requests).
- Four App Registrations created in the Entra ID tenant (Azure Active Directory). Follow the instructions in the [Authentication setup document](authentication/index.md) to configure authentication for the solution. 
    - Chat Client App Registration
    - Core API App Registration
    - Management Client App Registration
    - Management API App Registration
- User with the following role assignments:
    - Owner on the target subscription;
    - Owner on the two app registrations.

## Deployment steps

Follow the steps below to deploy the solution to your Azure subscription. You will be prompted to log in to your Azure account during the deployment process.

1. Ensure all the prerequisites are met, and that Docker Desktop is running.  

1. From a PowerShell prompt, execute the following to clone the repository:

    ```cmd
    git clone https://github.com/solliancenet/foundationallm.git
    git checkout release/0.4.0
    ```

3. Run the following commands to set the appropriate application registration settings for OIDC authentication. Please refer to the instructions on the [authentication setup page](authentication/index.md) to configure authentication for the solution and obtain the appropriate client Ids, scopes, and tenant Ids for the following steps.

    ```pwsh
    cd foundationallm
    cd deploy/starter

    az login            # Log into Azure CLI
    azd auth login      # Log into Azure Developer CLI

    azd env             # Set your target Subscription and Location

    azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
    azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
    azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

    azd env set ENTRA_CORE_API_CLIENT_ID <Core API Client Id>
    azd env set ENTRA_CORE_API_SCOPES <Core API Scope>
    azd env set ENTRA_CORE_API_TENANT_ID <Core API Tenant ID>

    azd env set ENTRA_MANAGEMENT_API_CLIENT_ID <Management API Client Id>
    azd env set ENTRA_MANAGEMENT_API_SCOPES <Management API Scope>
    azd env set ENTRA_MANAGEMENT_API_TENANT_ID <Management API Tenant ID>

    azd env set ENTRA_MANAGEMENT_UI_CLIENT_ID <Management UI Client Id>
    azd env set ENTRA_MANAGEMENT_UI_SCOPES <Management UI Scope>
    azd env set ENTRA_MANAGEMENT_UI_TENANT_ID <Management UI Tenant ID>

    azd env set ENTRA_VECTORIZATION_API_CLIENT_ID <Vectorization API Client Id>
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

    After setting the OIDC-specific settings in the AZD environment above, run `azd up` in the same folder location to build the Docker images, provision the infrastructure, update the App Configuration entries, deploy the API and web app services, and import files into the storage account.

    ```pwsh
    azd up
    ```

### Authentication setup

Follow the instructions in the [Authentication setup document](authentication/index.md) to configure authentication for the solution.
