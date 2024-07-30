param vnetName string
param destVnetId string

resource main 'Microsoft.Network/virtualNetworks@2024-01-01' existing = {
  name: vnetName
}

resource destinationToSourcePeering 'Microsoft.Network/virtualNetworks/virtualNetworkPeerings@2024-01-01' = {
  name: 'hub-to-vnet'
  parent: main
  properties: {
    allowForwardedTraffic: true
    allowGatewayTransit: true
    remoteVirtualNetwork: {
      id: destVnetId
    }
  }
}
