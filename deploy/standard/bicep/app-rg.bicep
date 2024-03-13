/** Inputs **/
@description('Action Group to use for alerts.')
param actionGroupId string

@description('Administrator Object Id')
param administratorObjectId string

@description('Chat UI OIDC Client Secret')
@secure()
param chatUiClientSecret string

@description('Core API OIDC Client Secret')
@secure()
param coreApiClientSecret string

@description('DNS Resource Group Name')
param dnsResourceGroupName string

@description('The environment name token used in naming resources.')
param environmentName string

@description('AKS namespace')
param k8sNamespace string

@description('Location used for all resources.')
param location string

@description('Log Analytics Workspace Id to use for diagnostics')
param logAnalyticsWorkspaceId string

@description('Log Analytics Workspace Resource Id to use for diagnostics')
param logAnalyticsWorkspaceResourceId string

@description('Management UI OIDC Client Secret')
@secure()
param managementUiClientSecret string

@description('Management API OIDC Client Secret')
@secure()
param managementApiClientSecret string

@description('Networking Resource Group Name')
param networkingResourceGroupName string

@description('OPS Resource Group name')
param opsResourceGroupName string

@description('Project Name, used in naming resources.')
param project string

@description('Storage Resource Group name')
param storageResourceGroupName string

@description('Timestamp used in naming nested deployments.')
param timestamp string = utcNow()

@description('Vectorization API OIDC Client Secret')
@secure()
param vectorizationApiClientSecret string
param vnetName string

/** Locals **/
@description('KeyVault resource suffix')
var opsResourceSuffix = '${project}-${environmentName}-${location}-ops'

@description('Storage resource suffix')
var storageResourceSuffix = '${project}-${environmentName}-${location}-storage'

@description('Resource Suffix used in naming resources.')
var resourceSuffix = '${project}-${environmentName}-${location}-${workload}'

@description('Tags for all resources')
var tags = {
  Environment: environmentName
  IaC: 'Bicep'
  Project: project
  Purpose: 'Services'
}

var backendServices = {
  'agent-factory-api': { displayName: 'AgentFactoryAPI' }
  'agent-hub-api': { displayName: 'AgentHubAPI' }
  'core-job': { displayName: 'CoreWorker' }
  'data-source-hub-api': { displayName: 'DataSourceHubAPI' }
  'gatekeeper-api': { displayName: 'GatekeeperAPI' }
  'gatekeeper-integration-api': { displayName: 'GatekeeperIntegrationAPI' }
  'langchain-api': { displayName: 'LangChainAPI' }
  'prompt-hub-api': { displayName: 'PromptHubAPI' }
  'semantic-kernel-api': { displayName: 'SemanticKernelAPI' }
  'vectorization-job': { displayName: 'VectorizationWorker' }
}

var chatUiService = { 'chat-ui': { displayName: 'Chat' } }
var coreApiService = { 'core-api': { displayName: 'CoreAPI' } }
var vectorizationApiService = { 'vectorization-api': { displayName: 'VectorizationAPI' } }

var managementUiService = { 'management-ui': { displayName: 'ManagementUI' } }
var managementApiService = { 'management-api': { displayName: 'ManagementAPI' } }

@description('Workload Token used in naming resources.')
var workload = 'svc'

/** Outputs **/

/** Data Sources **/
module network 'modules/utility/virtualNetworkData.bicep' = {
  name: 'network-${resourceSuffix}-${timestamp}'
  scope: resourceGroup(networkingResourceGroupName)
  params: {
    vnetName: vnetName
    subnetNames: [
      'FLLMBackend'
      'FLLMFrontend'
      'FLLMServices'
    ]
  }
}

var subnets = reduce(
  map(network.outputs.subnets, subnet => {
      '${subnet.name}': {
        id: subnet.id
        addressPrefix: subnet.addressPrefix
      }
    }),
  {},
  (cur, acc) => union(cur, acc)
)

/** Resources **/
resource identityDeployment 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  location: location
  name: 'uai-deployment-${resourceSuffix}'
  tags: tags
}

/** Nested Modules **/
module aksBackend 'modules/aks.bicep' = {
  name: 'aksBackend-${timestamp}'
  params: {
    actionGroupId: actionGroupId
    admnistratorObjectIds: [ administratorObjectId ]
    dnsResourceGroupName: dnsResourceGroupName
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    logAnalyticWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    networkingResourceGroupName: networkingResourceGroupName
    opsResourceGroupName: opsResourceGroupName
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'aks' ], zone.key))
    privateIpIngress: cidrHost(subnets.FLLMBackend.addressPrefix, 250)
    resourceSuffix: '${resourceSuffix}-backend'
    subnetId: subnets.FLLMBackend.id
    subnetIdPrivateEndpoint: subnets.FLLMServices.id
    tags: tags
    uaiDeploymentid: identityDeployment.id
  }
}

module aksFrontend 'modules/aks.bicep' = {
  name: 'aksFrontend-${timestamp}'
  params: {
    actionGroupId: actionGroupId
    admnistratorObjectIds: [ administratorObjectId ]
    dnsResourceGroupName: dnsResourceGroupName
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    logAnalyticWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    networkingResourceGroupName: networkingResourceGroupName
    opsResourceGroupName: opsResourceGroupName
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'aks' ], zone.key))
    privateIpIngress: cidrHost(subnets.FLLMFrontend.addressPrefix, 250)
    resourceSuffix: '${resourceSuffix}-frontend'
    subnetId: subnets.FLLMFrontend.id
    subnetIdPrivateEndpoint: subnets.FLLMServices.id
    tags: tags
    uaiDeploymentid: identityDeployment.id
  }
}

module dnsZones 'modules/utility/dnsZoneData.bicep' = {
  name: 'dnsZones-${timestamp}'
  scope: resourceGroup(dnsResourceGroupName)
  params: {
    location: location
  }
}

module eventgrid 'modules/eventgrid.bicep' = {
  name: 'eventgrid-${timestamp}'
  params: {
    actionGroupId: actionGroupId
    kvResourceSuffix: opsResourceSuffix
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    opsResourceGroupName: opsResourceGroupName
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'eventgrid' ], zone.key))
    resourceSuffix: resourceSuffix
    subnetId: subnets.FLLMServices.id
    topics: [ 'storage', 'vectorization', 'configuration' ]
    tags: tags
  }
}

module identityDeploymentRoleAssignments 'modules/utility/roleAssignments.bicep' = {
  name: 'identityDeploymentRoleAssignments-${timestamp}'
  params: {
    principalId: identityDeployment.properties.principalId
    roleDefinitionIds: {
      Contributor: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
      'Azure Kubernetes Service RBAC Cluster Admin': 'b1ff04bb-8a4e-4dc4-8eb5-8693973ce19b'
    }
  }
}

@batchSize(3)
module srBackend 'modules/service.bicep' = [for service in items(backendServices): {
  name: 'srBackend-${service.key}-${timestamp}'
  params: {
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksBackend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
  }
}]

@batchSize(3)
module srCoreApi 'modules/service.bicep' = [for service in items(coreApiService): {
  name: 'srCoreApi-${service.key}-${timestamp}'
  params: {
    clientSecret: coreApiClientSecret
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksBackend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
    useOidc: true
  }
}]

@batchSize(3)
module srChatUi 'modules/service.bicep' = [for service in items(chatUiService): {
  name: 'srChatUi-${service.key}-${timestamp}'
  params: {
    clientSecret: chatUiClientSecret
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksFrontend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
    useOidc: true
  }
}]

@batchSize(3)
module srManagementApi 'modules/service.bicep' = [for service in items(managementApiService): {
  name: 'srManagementApi-${service.key}-${timestamp}'
  params: {
    clientSecret: managementApiClientSecret
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksBackend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
    useOidc: true
  }
}]

@batchSize(3)
module srManagementUi 'modules/service.bicep' = [for service in items(managementUiService): {
  name: 'srManagementUi-${service.key}-${timestamp}'
  params: {
    clientSecret: managementUiClientSecret
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksFrontend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
    useOidc: true
  }
}]

@batchSize(3)
module srVectorizationApi 'modules/service.bicep' = [for service in items(vectorizationApiService): {
  name: 'srVectorizationApi-${service.key}-${timestamp}'
  params: {
    clientSecret: vectorizationApiClientSecret
    location: location
    namespace: k8sNamespace
    oidcIssuerUrl: aksBackend.outputs.oidcIssuerUrl
    opsResourceGroupName: opsResourceGroupName
    opsResourceSuffix: opsResourceSuffix
    resourceSuffix: resourceSuffix
    serviceName: service.key
    storageResourceGroupName: storageResourceGroupName
    tags: tags
    useOidc: false
  }
}]

module systemTopicAppConfig 'modules/config-system-topic.bicep' = {
  name: 'systemTopicAppConfig-${timestamp}'
  scope: resourceGroup(opsResourceGroupName)
  params: {
    actionGroupId: actionGroupId
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    resourceSuffix: resourceSuffix
    opsResourceSuffix: opsResourceSuffix
    tags: tags
  }
}

module systemTopicStorage 'modules/storage-system-topic.bicep' = {
  name: 'systemTopicStorage-${timestamp}'
  scope: resourceGroup(storageResourceGroupName)
  params: {
    actionGroupId: actionGroupId
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    resourceSuffix: resourceSuffix
    storageResourceSuffix: storageResourceSuffix
    tags: tags
  }
}

module systemTopicSubscriptionAppConfig 'modules/system-topic-subscription.bicep' = {
  name: 'systemTopicSubscriptionAppConfig-${timestamp}'
  scope: resourceGroup(opsResourceGroupName)
  params: {
    name: 'app-config'
    topicName: systemTopicAppConfig.outputs.name
    destinationTopicName: 'configuration'
    eventGridName: eventgrid.outputs.name
    appResourceGroup: resourceGroup().name
    includedEventTypes: [
      'Microsoft.AppConfiguration.KeyValueModified'
    ]
  }
}

module systemTopicSubscriptionStorage 'modules/system-topic-subscription.bicep' = {
  name: 'systemTopicSubscriptionStorage-${timestamp}'
  scope: resourceGroup(storageResourceGroupName)
  params: {
    name: 'resource-provider'
    topicName: systemTopicStorage.outputs.name
    destinationTopicName: 'storage'
    eventGridName: eventgrid.outputs.name
    appResourceGroup: resourceGroup().name
    filterPrefix: '/blobServices/default/containers/resource-provider/blobs'
    includedEventTypes: [
      'Microsoft.Storage.BlobCreated'
      'Microsoft.Storage.BlobDeleted'
    ]
  }
}
