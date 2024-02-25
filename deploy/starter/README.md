# Deployment

## Dependencies

Mac and Linux users can install the following dependencies locally to run the deployment. Windows users should use Ubuntu on [WSL](https://learn.microsoft.com/en-us/windows/wsl/install) to run all deployment steps. Other WSL Linux distributions may work, but these instructions have been validated with Ubuntu 18.04, 20.04, and 22.04.

- [azd CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [jq](https://jqlang.github.io/jq/download/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [git](https://git-scm.com/downloads)

### azd CLI

The Azure Developer CLI (azd) simplifies the process of building and deploying cloud applications. FoundationaLLM uses azd to build container images, release them to a private container registry contained in the deployment, and deploy them to Azure Container Apps. Consult the following links to learn more about azd.

- [What is the Azure Developer CLI?](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/overview)
- [Azure Friday Tutorial](https://www.youtube.com/watch?v=VTk-FhJyo7s)

## Deployment Instructions

Clone the FoundationaLLM repository

```pwsh
git clone -b release/0.4.0 https://github.com/solliancenet/foundationallm
```

Run the following commands to set the appropriate application registration settings for OIDC authentication. **Windows users should run them in WSL.**

```bash
cd foundationallm
cd deploy/starter

azd init
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

>**Note:** You need to manually generate a GUID for your instance ID.

```bash
uuidgen
```

After setting the OIDC-specific settings in the AZD environment above, run `azd up` in the same folder location to build the Docker images, provision the infrastructure, update the App Configuration entries, deploy the API and web app services, and import files into the storage account.

```bash
azd up
```

# Teardown

To tear down the environment, execute `azd down` in the same folder location.

```pwsh
azd down --purge
```

> Note the `--purge` argument in the command above. This ensures that resources that would otherwise be soft-deleted are instead completely purged from your Azure subscription.
