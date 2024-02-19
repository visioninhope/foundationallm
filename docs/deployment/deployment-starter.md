# Deployment - Starter

Foundationa**LLM** deploys into your own Azure Subscription. By default, it will deploy to Azure Container Apps (ACA), making it fast to get started. When you want to deploy to production at scale, you can also deploy to Azure Kubernetes Service (AKS).

## Prerequisites

- Azure Subscription (Subscription needs to be whitelisted for Azure OpenAI).
- Subscription access to Azure OpenAI service. Start here to [Request Access to Azure OpenAI Service](https://customervoice.microsoft.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR7en2Ais5pxKtso_Pz4b1_xUNTZBNzRKNlVQSFhZMU9aV09EVzYxWFdORCQlQCN0PWcu).
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- Docker Desktop.
- **Windows Users:** WSL with an Ubuntu 18.04/20.04/22.04 distribution
  - Other WSL Linux distributions may work, but these instructions have been validated with Ubuntu 18.04, 20.04, and 22.04 
- **Azure Deployment Tooling:** Windows users should install the following three dependencies in WSL; Mac and Linux users can install them locally
  - Azure CLI ([v2.51.0 or greater](https://docs.microsoft.com/cli/azure/install-azure-cli)).
  - Azure Developer CLI ([v1.5.1 or greater](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd))
  - jq ([jq-1.6 or greater](https://jqlang.github.io/jq/download/))
- Visual Studio 2022 (only needed if you plan to run/debug the solution locally).
- Minimum quota of 65 CPUs across all VM family types. Start here to [Manage VM Quotas](https://learn.microsoft.com/azure/quotas/per-vm-quota-requests).
- Two App Registrations created in the Entra ID tenant (Azure Active Directory).
- User with the following role assignments:
    - Owner on the target subscription;
    - Owner on the two app registrations.

## Deployment steps

Follow the steps below to deploy the solution to your Azure subscription. You will be prompted to log in to your Azure account during the deployment process.

1. Ensure all the prerequisites are met, and that Docker Desktop is running.  

2. From a PowerShell or bash prompt, execute the following to clone the latest release from the FLLM repository:

    ```cmd
    git clone -b release/0.4.0 https://github.com/solliancenet/foundationallm.git 
    ```

3. **Windows users should use Ubuntu in WSL, opened in the `foundationallm` directory, for the following steps; Mac and Linux users can follow them locally.**

    Run the following commands to set the appropriate application registration settings for OIDC authentication. Please refer to the instructions in the [Authentication setup document](authentication/index.md) to configure authentication for the solution and obtain the appropriate client Ids, scopes, and tenant Ids for the following steps.

    ```bash
    cd deploy/starter

    azd init

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

    >**Note:** You need to manually generate a GUID for `FOUNDATIONALLM_INSTANCE_ID`.

    ```bash
    uuidgen
    ```

    After setting the OIDC-specific settings in the AZD environment above, run `azd up` in the same folder location to build the Docker images, provision the infrastructure, update the App Configuration entries, deploy the API and web app services, and import files into the storage account.

    ```bash
    azd up
    ```

### Authentication setup

Follow the instructions on the [authentication setup page](authentication/index.md) to configure authentication for the solution.

## Update APIs and portals from local code changes

To update all APIs and portals from local code changes, run the following from the `./deploy/starter` folder in your locally cloned repository. Again, Windows users should use WSL.

```bash
azd deploy
```

To update an individual API or portal, suffix the command with the name of the service, as specified in the `./deploy/starter/azure.yaml` file.

```bash
azd deploy "prompt-hub-api"
```