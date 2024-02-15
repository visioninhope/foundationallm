#!/usr/bin/env pwsh

param (
    [parameter(Mandatory = $true)][string]$subscription
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"



$manifest = $(Get-Content -Raw -Path ../Deployment-Manifest.json | ConvertFrom-Json)

foreach ($resourceGroup in $manifest.resourceGroups.PSObject.Properties) {
    Write-Host "Finding Private Endpoints in Resource Group: $($resourceGroup.Name)" -ForegroundColor Green

    # temporary
    if ($resourceGroup.Name -ne "app") {
        continue
    }
    # end temporary

    $privateEndpointInterfaces = (
        az network private-endpoint list `
            --resource-group $resourceGroup.Value `
            --query "[].networkInterfaces[].id" `
            --subscription $subscription `
            --output json | `
            ConvertFrom-Json
    )

    $hosts = @{}
    foreach ($nic in $privateEndpointInterfaces) {
        $networkInterfaceFqdns = (
            az network nic show `
                --ids $nic `
                --query 'ipConfigurations[].{privateIPAddress:privateIPAddress,fqdn:privateLinkConnectionProperties.fqdns[0],groupId:privateLinkConnectionProperties.groupId}' `
                --output json | `
                ConvertFrom-Json
        )

        foreach ($fqdn in $networkInterfaceFqdns) {
            $hosts[$fqdn.fqdn] = $fqdn.privateIPAddress
        }
    }

    $hosts | ConvertTo-Json
}