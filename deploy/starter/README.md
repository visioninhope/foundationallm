# Deployment

Clone the FoundationaLLM repository

```pwsh
git clone https://github.com/solliancenet/foundationallm
```

Run the following command to set the appropriate application registration settings for OIDC authentication.

```pwsh
cd foundationallm
cd deploy/starter

azd env             # Set your target Subscription and Location

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>

azd env set ENTRA_CHAT_UI_CLIENT_ID <Chat UI Client Id>
azd env set ENTRA_CHAT_UI_SCOPES <Chat UI Scope>
azd env set ENTRA_CHAT_UI_TENANT_ID <Chat UI Tenant ID>
```

After setting the OIDC specific settings in the AZD environment above, run `azd up` in the same folder location to build the docker images, provision the infrastructure, update the configuration, deploy the API and web app services into container app instances, and import files into the storage account.

```pwsh
azd up
```

# Teardown

To tear down the environment, execute `azd down` in the same folder location.

```pwsh
azd down --purge
```

> Note the `--purge` argument in the command above. This ensures that resources that would otherwise be soft-deleted are instead completely purged from your Azure subscription.
