@description('Action Group Id for alerts')
param actionGroupId string

@description('Location for all resources')
param location string = resourceGroup().location

@description('Log Analytic Workspace Id to use for diagnostics')
param logAnalyticWorkspaceId string

@description('Resource suffix for all resources')
param resourceSuffix string

@description('Storage resource suffix')
param opsResourceSuffix string = resourceSuffix

@description('Tags for all resources')
param tags object

@description('Timestamp for nested deployments')
param timestamp string = utcNow()

/** Locals **/
@description('Metric alerts for the resource.')
var alerts = [
  // {
  //   description: 'Node CPU utilization greater than 95% for 1 hour'
  //   evaluationFrequency: 'PT5M'
  //   metricName: 'node_cpu_usage_percentage'
  //   name: 'node-cpu'
  //   operator: 'GreaterThan'
  //   severity: 3
  //   threshold: 95
  //   timeAggregation: 'Average'
  //   windowSize: 'PT5M'
  // }
  // {
  //   description: 'Node memory utilization greater than 95% for 1 hour'
  //   evaluationFrequency: 'PT5M'
  //   metricName: 'node_memory_working_set_percentage'
  //   name: 'node-memory'
  //   operator: 'GreaterThan'
  //   severity: 3
  //   threshold: 100
  //   timeAggregation: 'Average'
  //   windowSize: 'PT5M'
  // }
]

@description('The Resource logs to enable')
var logs = [
  // 'CassandraRequests'
  // 'ControlPlaneRequests'
  // 'DataPlaneRequests'
  // 'GremlinRequests'
  // 'MongoRequests'
  // 'PartitionKeyRUConsumption'
  // 'PartitionKeyStatistics'
  // 'QueryRuntimeStatistics'
  // 'TableApiRequests'
]

@description('The Resource Name')
var name = '${serviceType}-${resourceSuffix}'

@description('The Resource Service Type token')
var serviceType = 'eg'

@description('Formatted untruncated resource name')
var acFormattedName = toLower(replace('${acServiceType}-${opsResourceSuffix}', '-', ''))

@description('The Resource Name')
var appConfigAccountName = substring(acFormattedName,0,min([length(acFormattedName),24]))

@description('The Resource Service Type token')
var acServiceType = 'appconfig'

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2023-08-01-preview' existing = {
  name: appConfigAccountName
}

resource main 'Microsoft.EventGrid/systemTopics@2023-12-15-preview' = {
  name: name
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    source: appConfig.id
    topicType: 'Microsoft.AppConfiguration.ConfigurationStores'
  }
}

@description('Diagnostic settings for the resource')
resource diagnostics 'Microsoft.Insights/diagnosticSettings@2017-05-01-preview' = {
  scope: main
  name: 'diag-appconfig-${serviceType}'
  properties: {
    workspaceId: logAnalyticWorkspaceId
    logs: [for log in logs: {
      category: log
      enabled: true
    }]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
      }
    ]
  }
}

/** Nested Modules **/
@description('Metric alerts for the resource')
module metricAlerts 'utility/metricAlerts.bicep' = {
  name: 'alert-${main.name}-${timestamp}'
  params: {
    actionGroupId: actionGroupId
    alerts: alerts
    metricNamespace: 'Microsoft.EventGrid/namespaces'
    nameSuffix: name
    serviceId: main.id
    tags: tags
  }
}

output name string = main.name
