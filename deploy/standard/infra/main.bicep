targetScope = 'subscription'

param administratorObjectId string
param allowedExternalCidr string
param authAppRegistrationClientId string
param authAppRegistrationInstance string
param authAppRegistrationTenantId string
param backendSystemPoolMinCount int
param backendSystemPoolMaxCount int
param backendSystemPoolVmSize string
param backendUserPoolMinCount int
param backendUserPoolMaxCount int
param backendUserPoolVmSize string
param cidrVnet string
param createDate string = utcNow('u')
param environmentName string
param externalNetworkingResourceGroupName string = ''
param existingOpenAiInstanceName string = ''
param existingOpenAiInstanceRg string = ''
param existingOpenAiInstanceSub string = ''
param frontendSystemPoolMinCount int
param frontendSystemPoolMaxCount int
param frontendSystemPoolVmSize string
param frontendUserPoolMinCount int
param frontendUserPoolMaxCount int
param frontendUserPoolVmSize string
param instanceId string
param location string
param networkName string = ''
param project string
param registry string
param services array
param timestamp string = utcNow()
param userPortalHostname string
param managementPortalHostname string
param coreApiHostname string
param managementApiHostname string

param hubResourceGroup string
param hubSubscriptionId string = subscription().subscriptionId
param hubVnetName string

// Locals
var abbrs = loadJsonContent('./abbreviations.json')
var k8sNamespace = 'fllm'
var useExternalNetworking = !empty(externalNetworkingResourceGroupName)

var existingOpenAiInstance = {
  name: existingOpenAiInstanceName
  resourceGroup: existingOpenAiInstanceRg
  subscriptionId: existingOpenAiInstanceSub
}

var tags = {
  'compute-type': 'aks'
  'create-date': createDate
  'azd-env-name': environmentName
  'iac-type': 'bicep'
  'project-name': project
}

// TODO: BYO Resource Groups
var resourceGroups = union(defaultResourceGroups, externalResourceGroups)
var externalResourceGroups = externalNetworkingResourceGroup
var externalNetworkingResourceGroup = useExternalNetworking ? { net: externalNetworkingResourceGroupName } : {}

var defaultResourceGroups = reduce(
  map(
    workloads,
    workload => { '${workload}': namer(abbrs.resourcesResourceGroups, environmentName, location, workload, project) }
  ),
  {},
  (cur, next) => union(cur, next)
)

var workloads = [
  'app'
  'auth'
  'data'
  'jbx'
  'net'
  'oai'
  'ops'
  'storage'
  'vec'
]

// Functions
func namer(resourceAbbr string, env string, region string, workloadName string, projectId string) string =>
  '${resourceAbbr}${env}-${region}-${workloadName}-${projectId}'

// Resources
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = [
  for rgName in items(resourceGroups): {
    name: rgName.value
    location: location
    tags: tags
  }
]

// Nested Deployments
module app 'app-rg.bicep' = {
  dependsOn: [rg, networking, ops, storage]
  name: 'app-${timestamp}'
  scope: resourceGroup(resourceGroups.app)
  params: {
    actionGroupId: ops.outputs.actionGroupId
    administratorObjectId: administratorObjectId
    backendSystemPoolMinCount: backendSystemPoolMinCount
    backendSystemPoolMaxCount: backendSystemPoolMaxCount
    backendSystemPoolVmSize: backendSystemPoolVmSize
    backendUserPoolMinCount: backendUserPoolMinCount
    backendUserPoolMaxCount: backendUserPoolMaxCount
    backendUserPoolVmSize: backendUserPoolVmSize
    environmentName: environmentName
    frontendSystemPoolMinCount: frontendSystemPoolMinCount
    frontendSystemPoolMaxCount: frontendSystemPoolMaxCount
    frontendSystemPoolVmSize: frontendSystemPoolVmSize
    frontendUserPoolMinCount: frontendUserPoolMinCount
    frontendUserPoolMaxCount: frontendUserPoolMaxCount
    frontendUserPoolVmSize: frontendUserPoolVmSize    
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    k8sNamespace: k8sNamespace
    location: location
    logAnalyticsWorkspaceId: ops.outputs.logAnalyticsWorkspaceId
    logAnalyticsWorkspaceResourceId: ops.outputs.logAnalyticsWorkspaceId
    networkingResourceGroupName: resourceGroups.net
    openAiResourceGroupName: resourceGroups.oai
    opsResourceGroupName: resourceGroups.ops
    project: project
    services: services
    storageResourceGroupName: resourceGroups.storage
    vectorizationResourceGroupName: resourceGroups.vec
    vnetName: networking.outputs.vnetName
  }
}

module auth 'auth-rg.bicep' = {
  dependsOn: [rg, app]
  name: 'auth-${timestamp}'
  scope: resourceGroup(resourceGroups.auth)
  params: {
    actionGroupId: ops.outputs.actionGroupId
    administratorObjectId: administratorObjectId
    appResourceGroupName: resourceGroups.app
    authAppRegistrationClientId: authAppRegistrationClientId
    authAppRegistrationInstance: authAppRegistrationInstance
    authAppRegistrationTenantId: authAppRegistrationTenantId
    environmentName: environmentName
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    instanceId: instanceId
    k8sNamespace: k8sNamespace
    location: location
    logAnalyticsWorkspaceId: ops.outputs.logAnalyticsWorkspaceId
    opsResourceGroupName: resourceGroups.ops
    project: project
    vnetId: networking.outputs.vnetId
  }
}

module networking 'networking-rg.bicep' = {
  dependsOn: [rg]
  name: 'networking-${timestamp}'
  scope: resourceGroup(resourceGroups.net)
  params: {
    cidrVnet: cidrVnet
    allowedExternalCidr: allowedExternalCidr
    environmentName: environmentName
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    hubVnetName: hubVnetName
    location: location
    networkName: networkName
    project: project
  }
}

module openai 'openai-rg.bicep' = {
  dependsOn: [rg, networking]
  name: 'openai-${timestamp}'
  scope: resourceGroup(resourceGroups.oai)
  params: {
    actionGroupId: ops.outputs.actionGroupId
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    environmentName: environmentName
    existingOpenAiInstance: existingOpenAiInstance
    location: location
    logAnalyticsWorkspaceId: ops.outputs.logAnalyticsWorkspaceId
    opsKeyVaultName: ops.outputs.keyVaultName
    opsResourceGroupName: resourceGroups.ops
    project: project
    vnetId: networking.outputs.vnetId
  }
}

module ops 'ops-rg.bicep' = {
  dependsOn: [rg, networking]
  name: 'ops-${timestamp}'
  scope: resourceGroup(resourceGroups.ops)
  params: {
    administratorObjectId: administratorObjectId
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    environmentName: environmentName
    location: location
    project: project
    vnetId: networking.outputs.vnetId
  }
}

module storage 'storage-rg.bicep' = {
  dependsOn: [rg, networking]
  name: 'storage-${timestamp}'
  scope: resourceGroup(resourceGroups.storage)
  params: {
    actionGroupId: ops.outputs.actionGroupId
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    environmentName: environmentName
    location: location
    logAnalyticsWorkspaceId: ops.outputs.logAnalyticsWorkspaceId
    project: project
    vnetId: networking.outputs.vnetId
  }
}

module vec 'vec-rg.bicep' = {
  dependsOn: [rg, networking]
  name: 'vec-${timestamp}'
  scope: resourceGroup(resourceGroups.vec)
  params: {
    actionGroupId: ops.outputs.actionGroupId
    hubResourceGroup: hubResourceGroup
    hubSubscriptionId: hubSubscriptionId
    environmentName: environmentName
    location: location
    logAnalyticsWorkspaceId: ops.outputs.logAnalyticsWorkspaceId
    project: project
    vnetId: networking.outputs.vnetId
  }
}

output ADMIN_GROUP_OBJECT_ID string = administratorObjectId

output AZURE_CONTENT_SAFETY_ENDPOINT string = openai.outputs.azureContentSafetyEndpoint
output AZURE_OPENAI_ENDPOINT string = openai.outputs.azureOpenAiEndpoint
output AZURE_OPENAI_ID string = openai.outputs.azureOpenAiId
output AZURE_STORAGE_ACCOUNT_NAME string = storage.outputs.storageAccountName

output FOUNDATIONALLM_PROJECT string = project
output FOUNDATIONALLM_K8S_NS string = k8sNamespace
output FOUNDATIONALLM_REGISTRY string = registry

output FLLM_APP_RG     string = resourceGroups.app
output FLLM_AUTH_RG    string = resourceGroups.auth
output FLLM_DATA_RG    string = resourceGroups.data
output FLLM_JBX_RG     string = resourceGroups.jbx
output FLLM_NET_RG     string = resourceGroups.net
output FLLM_OAI_RG     string = resourceGroups.oai
output FLLM_OPS_RG     string = resourceGroups.ops
output FLLM_STORAGE_RG string = resourceGroups.storage
output FLLM_VEC_RG     string = resourceGroups.vec

output FLLM_OPS_KV string = ops.outputs.keyVaultName

output FLLM_USER_PORTAL_HOSTNAME string = userPortalHostname
output FLLM_MGMT_PORTAL_HOSTNAME string = managementPortalHostname
output FLLM_CORE_API_HOSTNAME string = coreApiHostname
output FLLM_MGMT_API_HOSTNAME string = managementApiHostname

output FOUNDATIONALLM_VNET_NAME string = networking.outputs.vnetName
output FOUNDATIONALLM_VNET_ID string = networking.outputs.vnetId
output FOUNDATIONALLM_HUB_VNET_ID string = networking.outputs.hubVnetId

output SERVICE_GATEKEEPER_API_ENDPOINT_URL string = 'http://gatekeeper-api/gatekeeper/'
output SERVICE_GATEKEEPER_INTEGRATION_API_ENDPOINT_URL string = 'http://gatekeeper-integration-api/gatekeeperintegration'
output SERVICE_GATEWAY_ADAPTER_API_ENDPOINT_URL string = 'http://gateway-adapter-api/gatewayadapter'
output SERVICE_GATEWAY_API_ENDPOINT_URL string = 'http://gateway-api/gateway'
output SERVICE_LANGCHAIN_API_ENDPOINT_URL string = 'http://langchain-api/langchain'
output SERVICE_ORCHESTRATION_API_ENDPOINT_URL string = 'http://orchestration-api/orchestration'
output SERVICE_SEMANTIC_KERNEL_API_ENDPOINT_URL string = 'http://semantic-kernel-api/semantickernel'
output SERVICE_STATE_API_ENDPOINT_URL string = 'http://state-api/state'
output SERVICE_VECTORIZATION_API_ENDPOINT_URL string = 'http://vectorization-api/vectorization'
output SERVICE_VECTORIZATION_JOB_ENDPOINT_URL string = 'http://vectorization-job/vectorization'
output SERVICE_CHAT_UI_ENDPOINT_URL string = 'https://${userPortalHostname}'
output SERVICE_CORE_API_ENDPOINT_URL string = 'https://${coreApiHostname}'
output SERVICE_MANAGEMENT_API_ENDPOINT_URL string = 'https://${managementApiHostname}'
output SERVICE_MANAGEMENT_UI_ENDPOINT_URL string = 'https://${managementPortalHostname}'
