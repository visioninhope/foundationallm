param keyvaultName string
param location string = resourceGroup().location
param name string
param sku string = 'S0'
param tags object = {}

resource contentSafety 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
  name: name
  location: location
  sku: {
    name: sku
  }
  kind: 'ContentSafety'
  properties: {
    customSubDomainName: name
    publicNetworkAccess: 'Enabled'
  }
  tags: tags
}

resource keyvault 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: keyvaultName
}

resource apiKeySecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: 'foundationallm-apiendpoints-azurecontentsafety-apikey'
  parent: keyvault
  tags: tags
  properties: {
    value: contentSafety.listKeys().key1
  }
}

output endpoint string = contentSafety.properties.endpoint
output keySecretName string = apiKeySecret.name
output keySecretRef string = apiKeySecret.properties.secretUri
output name string = contentSafety.name
