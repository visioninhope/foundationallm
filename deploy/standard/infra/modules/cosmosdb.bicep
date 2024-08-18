/** Inputs **/
@description('Action Group Id for alerts')
param actionGroupId string

@description('Location for all resources')
param location string

@description('Log Analytic Workspace Id to use for diagnostics')
param logAnalyticWorkspaceId string

@description('Private DNS Zones for private endpoint')
param privateDnsZones array

@description('Resource suffix for all resources')
param resourceSuffix string

@description('Subnet Id for private endpoint')
param subnetId string

@description('Tags for all resources')
param tags object

@description('Timestamp for nested deployments')
param timestamp string = utcNow()

/** Locals **/
@description('Metric alerts for the resource')
var alerts = [
  {
    description: 'Service availability less than 99% for 1 hour'
    evaluationFrequency: 'PT1M'
    metricName: 'ServiceAvailability'
    name: 'storageUsage'
    operator: 'LessThan'
    severity: 0
    threshold: 99
    timeAggregation: 'Average'
    windowSize: 'PT1H'
  }
]

@description('The container configurations for the database containers.')
var containers = [
  {
    name: 'UserSessions'
    partitionKey: {
      paths: [
        '/upn'
      ]
    }
    maxThroughput: 4000
    defaultTtl: null
  }
  {
    name: 'Sessions'
    partitionKey: {
      paths: [
        '/sessionId'
      ]
    }
    maxThroughput: 5000
    defaultTtl: null
  }
  {
    name: 'State'
    partitionKey: {
      paths: [
        '/operation_id'
      ]
    }
    maxThroughput: 2000
    defaultTtl: 604800
  }
  {
    name: 'leases'
    partitionKey: {
      paths: [
        '/id'
      ]
    }
    maxThroughput: 1000
    defaultTtl: null
  }
]

@description('The Resource logs to enable')
var logs = [
  'CassandraRequests'
  'ControlPlaneRequests'
  'DataPlaneRequests'
  'GremlinRequests'
  'MongoRequests'
  'PartitionKeyRUConsumption'
  'PartitionKeyStatistics'
  'QueryRuntimeStatistics'
  'TableApiRequests'
]

@description('The Resource Name')
var name = '${serviceType}-${resourceSuffix}'

@description('The Resource Service Type token')
var serviceType = 'cdb'

/** Outputs **/

/** Resources **/
@description('The database account.')
resource main 'Microsoft.DocumentDB/databaseAccounts@2023-09-15' = {
  kind: 'GlobalDocumentDB'
  location: location
  name: name
  tags: tags

  identity: {
    type: 'None'
  }

  properties: {
    createMode: 'Default'
    databaseAccountOfferType: 'Standard'
    defaultIdentity: 'FirstPartyIdentity'
    disableKeyBasedMetadataWriteAccess: false
    capabilities: []
    cors: []
    disableLocalAuth: false
    enableAnalyticalStorage: false
    enableAutomaticFailover: false
    enableBurstCapacity: false
    enableFreeTier: false
    enableMultipleWriteLocations: false
    enablePartitionMerge: false
    ipRules: []
    isVirtualNetworkFilterEnabled: false
    minimalTlsVersion: 'Tls12'
    networkAclBypass: 'None'
    networkAclBypassResourceIds: []
    publicNetworkAccess: 'Disabled'
    virtualNetworkRules: []

    analyticalStorageConfiguration: {
      schemaType: 'WellDefined'
    }

    backupPolicy: {
      type: 'Continuous'

      continuousModeProperties: {
        tier: 'Continuous30Days'
      }
    }

    capacity: {
      totalThroughputLimit: 12000
    }

    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
      maxIntervalInSeconds: 5
      maxStalenessPrefix: 100
    }

    locations: [
      {
        failoverPriority: 0
        isZoneRedundant: false
        locationName: location
      }
    ]
  }
}

@description('The database in the account')
resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-09-15' = {
  name: 'database'
  parent: main

  properties: {
    resource: {
      id: 'database'
    }
  }
}

@description('Diagnostic settings for the resource')
resource diagnostics 'Microsoft.Insights/diagnosticSettings@2017-05-01-preview' = {
  scope: main
  name: 'diag-${serviceType}'
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

resource cosmosContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = [
  for c in containers: if (c.defaultTtl == null) {
    name: c.name
    parent: database

    properties: {
      options: {
        autoscaleSettings: {
          maxThroughput: c.maxThroughput
        }
      }
      resource: {
        id: c.name
        indexingPolicy: {
          indexingMode: 'consistent'
          automatic: true

          excludedPaths: [
            {
              path: '/"_etag"/?'
            }
          ]

          includedPaths: [
            {
              path: '/*'
            }
          ]
        }

        partitionKey: {
          kind: 'Hash'
          paths: c.partitionKey.paths
          version: 2
        }

        uniqueKeyPolicy: {
          uniqueKeys: []
        }

        conflictResolutionPolicy: {
          conflictResolutionPath: '/_ts'
          mode: 'LastWriterWins'
        }
      }
    }
  }
]

resource cosmosContainerWithTtl 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = [
  for c in containers: if (c.defaultTtl != null) {
    name: c.name
    parent: database

    properties: {
      options: {
        autoscaleSettings: {
          maxThroughput: c.maxThroughput
        }
      }
      resource: {
        id: c.name
        indexingPolicy: {
          indexingMode: 'consistent'
          automatic: true

          excludedPaths: [
            {
              path: '/"_etag"/?'
            }
          ]

          includedPaths: [
            {
              path: '/*'
            }
          ]
        }

        partitionKey: {
          kind: 'Hash'
          paths: c.partitionKey.paths
          version: 2
        }

        defaultTtl: c.?defaultTtl

        uniqueKeyPolicy: {
          uniqueKeys: []
        }

        conflictResolutionPolicy: {
          conflictResolutionPath: '/_ts'
          mode: 'LastWriterWins'
        }
      }
    }
  }
]

/** Nested Modules **/
@description('Metric alerts for the resource')
module metricAlerts 'utility/metricAlerts.bicep' = {
  name: 'alert-${main.name}-${timestamp}'
  params: {
    actionGroupId: actionGroupId
    alerts: alerts
    metricNamespace: 'Microsoft.DocumentDB/DatabaseAccounts'
    nameSuffix: name
    serviceId: main.id
    tags: tags
  }
}

@description('Private endpoint for the resource')
module privateEndpoint 'utility/privateEndpoint.bicep' = {
  name: 'pe-${main.name}-${timestamp}'
  params: {
    groupId: 'Sql'
    location: location
    privateDnsZones: privateDnsZones
    subnetId: subnetId
    tags: tags

    service: {
      id: main.id
      name: main.name
    }
  }
}
