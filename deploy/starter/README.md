# Deployment

## Dependencies

Please install the following dependencies to deploy FLLM.

- [PowerShell 7](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.4) (required for `pwsh` to work in the AZD hooks as an alias to powershell)
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

Run the following commands to set the appropriate application registration settings for OIDC authentication.

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

```bash
azd up
```

# Teardown

To tear down the environment, execute `azd down` in the same folder location.

```pwsh
azd down --purge
```

> Note the `--purge` argument in the command above. This ensures that resources that would otherwise be soft-deleted are instead completely purged from your Azure subscription.
