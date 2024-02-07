#!/bin/bash

echo "Loading azd .env file from current environment"

# Use the `get-values` azd command to retrieve environment variables from the `.env` file
while IFS='=' read -r key value; do
    value=$(echo "$value" | sed 's/^"//' | sed 's/"$//')
    export "$key=$value"
done <<EOF
$(azd env get-values) 
EOF

envsubst < ./config/appconfig.template.json > ./config/appconfig.json

az appconfig kv import -n ${AZURE_APP_CONFIG_NAME} -s file --path ./config/appconfig.json --format json -y

az storage azcopy blob upload -c agents --account-name $AZURE_STORAGE_ACCOUNT_NAME -s "../common/data/agents/*" --recursive --only-show-errors
az storage azcopy blob upload -c data-sources --account-name $AZURE_STORAGE_ACCOUNT_NAME -s "../common/data/data-sources/*" --recursive --only-show-errors
az storage azcopy blob upload -c foundationallm-source --account-name $AZURE_STORAGE_ACCOUNT_NAME -s "../common/data/foundationallm-source/*" --recursive --only-show-errors
az storage azcopy blob upload -c prompts --account-name $AZURE_STORAGE_ACCOUNT_NAME -s "../common/data/prompts/*" --recursive --only-show-errors
az storage azcopy blob upload -c resource-provider --account-name $AZURE_STORAGE_ACCOUNT_NAME -s "../common/data/resource-provider/*" --recursive --only-show-errors
