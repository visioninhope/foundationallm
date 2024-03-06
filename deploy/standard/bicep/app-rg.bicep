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

@description('Virtual Network ID, used to find the subnet IDs.')
param vnetId string

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
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'aks' ], zone.key))
    resourceSuffix: '${resourceSuffix}-backend'
    subnetId: '${vnetId}/subnets/FLLMBackend'
    subnetIdPrivateEndpoint: '${vnetId}/subnets/FLLMServices'
    tags: tags
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
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'aks' ], zone.key))
    resourceSuffix: '${resourceSuffix}-frontend'
    subnetId: '${vnetId}/subnets/FLLMFrontend'
    subnetIdPrivateEndpoint: '${vnetId}/subnets/FLLMServices'
    tags: tags
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
    dnsResourceGroupName: dnsResourceGroupName
    kvResourceSuffix: opsResourceSuffix
    location: location
    logAnalyticWorkspaceId: logAnalyticsWorkspaceId
    logAnalyticWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    networkingResourceGroupName: networkingResourceGroupName
    opsResourceGroupName: opsResourceGroupName
    privateDnsZones: filter(dnsZones.outputs.ids, (zone) => contains([ 'eventgrid' ], zone.key))
    resourceSuffix: resourceSuffix
    subnetId: '${vnetId}/subnets/FLLMServices'
    topics: [ 'storage', 'vectorization', 'configuration' ]
    tags: tags
  }
}

@batchSize(3)
module serviceResourcesBackend 'modules/service.bicep' = [for service in items(backendServices): {
  name: 'serviceResourcesBackend-${service.key}-${timestamp}'
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
module serviceResourcesCoreApi 'modules/service.bicep' = [for service in items(coreApiService): {
  name: 'serviceResourcesCoreApi-${service.key}-${timestamp}'
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
module serviceResourcesChatUi 'modules/service.bicep' = [for service in items(chatUiService): {
  name: 'serviceResourcesChatUi-${service.key}-${timestamp}'
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
module serviceResourcesManagementApi 'modules/service.bicep' = [for service in items(managementApiService): {
  name: 'serviceResourcesManagementApi-${service.key}-${timestamp}'
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
module serviceResourcesManagementUi 'modules/service.bicep' = [for service in items(managementUiService): {
  name: 'serviceResourcesManagementUi-${service.key}-${timestamp}'
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
module serviceResourcesVectorizationApi 'modules/service.bicep' = [for service in items(vectorizationApiService): {
  name: 'serviceResourcesVectorizationApi-${service.key}-${timestamp}'
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
  name: 'ssTopic-${timestamp}'
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
  name: 'ssTopic-${timestamp}'
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
