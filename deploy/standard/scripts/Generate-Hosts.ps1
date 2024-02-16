#!/usr/bin/env pwsh

param (
    [parameter(Mandatory = $true)][string]$subscription
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

function EnsureSuccess {
    param (
        [parameter(Mandatory = $true)][int]$exitcode,
        [parameter(Mandatory = $true)][string]$message
    )

    if ($exitcode -ne 0) {
        Write-Host $message -ForegroundColor Red
        exit $exitcode
    }
}

function Get-Hosts-Default-Strategy {
    param (
        [parameter(Mandatory = $true)][string]$id,
        [parameter(Mandatory = $true)][hashtable]$hosts
    )

    $networkInterfaceFqdns = az network nic show `
        --ids $id `
        --query 'ipConfigurations[].{
                fqdn:privateLinkConnectionProperties.fqdns[0],
                groupId:privateLinkConnectionProperties.groupId,
                nicId:id,
                nicName:name,
                peId:privateEndpoint.id
                privateIPAddress:privateIPAddress
            }' `
        --output json | `
        ConvertFrom-Json
    EnsureSuccess $LASTEXITCODE "Failed to get Network Interface FQDNs"

    foreach ($fqdn in $networkInterfaceFqdns) {
        $hosts[$fqdn.fqdn] = $fqdn.privateIPAddress
    }
}

$manifest = $(Get-Content -Raw -Path ../Deployment-Manifest.json | ConvertFrom-Json)

foreach ($resourceGroup in $manifest.resourceGroups.PSObject.Properties) {
    Write-Host "Finding Private Endpoints in Resource Group: $($resourceGroup.Name)" -ForegroundColor Green

    $privateEndpoints = az network private-endpoint list `
        --resource-group $resourceGroup.Value `
        --query "[].{
            groupId:privateLinkServiceConnections[0].groupIds[0],
            nicId:networkInterfaces[0].id,
            peId:id,
            peName:name,
            serviceId:privateLinkServiceConnections[0].privateLinkServiceId
        }" `
        --subscription $subscription `
        --output json   `
    | ConvertFrom-Json
    EnsureSuccess $LASTEXITCODE "Failed to get Private Endpoints in Resource Group: $($resourceGroup.Name)"

    if ($null -eq $privateEndpoints -or $privateEndpoints.Count -lt 1) {
        Write-Host "No Private Endpoints found in Resource Group: $($resourceGroup.Name)" -ForegroundColor Yellow
        continue
    }

    $hosts = @{}
    foreach ($privateEndpoint in $privateEndpoints) {
        switch ($privateEndpoint.groupId) {
            "management" {
                Write-Host "Found AKS Management Private Endpoint: $($privateEndpoint.peName)" -ForegroundColor Yellow
                $aksInstance = az aks list `
                    --resource-group $resourceGroup.Value `
                    --output json | `
                    ConvertFrom-Json | `
                    Where-Object { $_.id -eq $privateEndpoint.serviceId } | `
                    Select-Object -Property id, name, privateFqdn | `
                    Add-Member -MemberType NoteProperty -Name nicId -Value $privateEndpoint.nicId -PassThru

                EnsureSuccess $LASTEXITCODE "Failed to get AKS Instances for Private Endpoint: $($privateEndpoint.peName)"

                $networkInterfaceFqdns = az network nic show `
                    --ids $aksInstance.nicId `
                    --query 'ipConfigurations[].{privateIPAddress:privateIPAddress}' `
                    --output json | `
                    ConvertFrom-Json | `
                    Add-Member -MemberType NoteProperty -Name fqdn -Value $aksInstance.privateFqdn -PassThru
                EnsureSuccess $LASTEXITCODE "Failed to get Network Interface FQDNs for AKS Instance: $($aksInstance.name)"

                foreach ($fqdn in $networkInterfaceFqdns) {
                    $hosts[$fqdn.fqdn] = $fqdn.privateIPAddress
                }
            }
            default {
                Get-Hosts-Default-Strategy -id $privateEndpoint.nicId -hosts $hosts
            }
        }
    }

    $hosts | ConvertTo-Json
}

# $peInstances = $(
#     az network private-endpoint list `
#         --resource-group $resourceGroups.app `
#         --query "[].{groupId:privateLinkServiceConnections[0].groupIds[0],peName:name,aksId:privateLinkServiceConnections[0].privateLinkServiceId,nicId:networkInterfaces[0].id,peId:id}" `
#         --output json | `
#         ConvertFrom-Json
# )

# $nicInstances = $(
#     az network nic list `
#         --resource-group $resourceGroups.app `
#         --query "[].{nicId:id, nicName:name, privateIpAddress:ipConfigurations[0].privateIPAddress,peId:privateEndpoint.id}" `
#         --output json | `
#         ConvertFrom-Json
# )

####

# function GetAksPrivateIPMapping($aksInstance, $peInstances, $nicInstances) {
#     $peInstance = $peInstances | Where-Object { $_.aksId -eq $aksInstance.aksId }
#     $nicInstance = $nicInstances | Where-Object { $_.peId -eq $peInstance.peId }

#     return @{
#         privateIPAddress = $nicInstance.privateIpAddress
#         fqdn             = $aksInstance.privateFqdn
#         groupId          = $peInstance.groupId
#     }
# }

# Write-Host "Getting AKS Instances, Private Endpoints and NICs"
# $aksInstances = $(
#     az aks list `
#         --resource-group $resourceGroups.app `
#         --query "[].{aksName:name,privateFqdn:privateFqdn,aksId:id}" `
#         --output json | `
#         ConvertFrom-Json
# )




# Write-Host "Found $($aksInstances.Length) AKS Instances" -ForegroundColor Yellow
# for ($i = 0; $i -lt $aksInstances.Length; $i++) {
#     $aksInstance = $aksInstances[$i]
#     Write-Host "AKS Instance $($i): $($aksInstance.aksName)" -ForegroundColor Blue
#     $privateIpMapping = GetAksPrivateIPMapping $aksInstance $peInstances $nicInstances
#     $tokens.Add("aksFqdn$($i)", $aksInstance.privateFqdn)
#     $tokens.Add("aksPrivateIp$($i)", $privateIpMapping.privateIPAddress)
# }