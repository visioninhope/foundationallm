#!/usr/bin/env pwsh

param (
    [parameter(Mandatory = $true)][string]$subscription
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

function Invoke-AndRequireSuccess {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string]$Message,

        [Parameter(Mandatory = $true, Position = 1)]
        [ScriptBlock]$ScriptBlock
    )

    $result = & $ScriptBlock

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed: ${message}" -ForegroundColor Red
        exit $LASTEXITCODE
    }

    return $result
}

function Get-Hosts-Aks-Strategy {
    param(
        [parameter(Mandatory = $true)][string]$privateEndpointName,
        [parameter(Mandatory = $true)][string]$privateEndpointServiceId,
        [parameter(Mandatory = $true)][string]$privateEndpointNetworkInterfaceId
    )

    $aksInstance = Invoke-AndRequireSuccess "Get AKS Instances for Private Endpoint: $($privateEndpointName)" {
        az aks list `
            --resource-group $resourceGroup.Value `
            --query '[].{id:id, name:name, privateFqdn:privateFqdn}' `
            --output json | `
            ConvertFrom-Json | `
            Where-Object { $_.id -eq $privateEndpointServiceId } | `
            Select-Object -Property name, @{Name = 'fqdn'; Expression = { $_.privateFqdn } } | `
            Add-Member -MemberType NoteProperty -Name nicId -Value $privateEndpointNetworkInterfaceId -PassThru
    }

    $networkInterfaceFqdns = Invoke-AndRequireSuccess "Get Network Interface FQDNs for AKS Instance: $($aksInstance.name)" {
        az network nic show `
            --ids $aksInstance.nicId `
            --query 'ipConfigurations[].{privateIPAddress:privateIPAddress}' `
            --output json | `
            ConvertFrom-Json | `
            Add-Member -MemberType NoteProperty -Name fqdn -Value $aksInstance.fqdn -PassThru
    }

    return $networkInterfaceFqdns
}

function Get-Hosts-Default-Strategy {
    param (
        [parameter(Mandatory = $true)][string]$id
    )

    $networkInterfaceFqdns = Invoke-AndRequireSuccess "Get Network Interface FQDNs" {
        az network nic show `
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
    }

    return $networkInterfaceFqdns
}

$manifest = $(Get-Content -Raw -Path ../Deployment-Manifest.json | ConvertFrom-Json)

$hosts = @{}
foreach ($resourceGroup in $manifest.resourceGroups.PSObject.Properties) {
    Write-Host "Finding Private Endpoints in Resource Group: $($resourceGroup.Name)" -ForegroundColor Green

    $privateEndpoints = Invoke-AndRequireSuccess "Get Private Endpoints in Resource Group: $($resourceGroup.Name)" {
        az network private-endpoint list `
            --resource-group $resourceGroup.Value `
            --query "[].{
                groupId:privateLinkServiceConnections[0].groupIds[0],
                nicId:networkInterfaces[0].id,
                peName:name,
                serviceId:privateLinkServiceConnections[0].privateLinkServiceId
            }" `
            --subscription $subscription `
            --output json | `
            ConvertFrom-Json
    }

    if ($null -eq $privateEndpoints -or $privateEndpoints.Count -lt 1) {
        Write-Host "No Private Endpoints found in Resource Group: $($resourceGroup.Name)" -ForegroundColor Yellow
        continue
    }

    foreach ($privateEndpoint in $privateEndpoints) {
        $networkInterfaceFqdns = @()
        switch ($privateEndpoint.groupId) {
            "management" {
                Write-Host "Found AKS Management Private Endpoint: $($privateEndpoint.peName)" -ForegroundColor Yellow
                $networkInterfaceFqdns = Get-Hosts-Aks-Strategy `
                    -privateEndpointName $privateEndpoint.peName `
                    -privateEndpointServiceId $privateEndpoint.serviceId `
                    -privateEndpointNetworkInterfaceId $privateEndpoint.nicId
            }
            default {
                Write-Host "Found Default Private Endpoint: $($privateEndpoint.peName)" -ForegroundColor Yellow
                $networkInterfaceFqdns = Get-Hosts-Default-Strategy -id $privateEndpoint.nicId
            }
        }

        foreach ($fqdn in $networkInterfaceFqdns) {
            $hosts[$fqdn.fqdn] = $fqdn.privateIPAddress
        }
    }
}

$hostFile = @()
foreach ($endpoint in $hosts.GetEnumerator()) {
    $hostFile += "$($endpoint.Value)  $($endpoint.Key)"
}

Write-Host "Writing hosts file" -ForegroundColor Green
$hostFile | Sort-Object | Out-File -FilePath "../config/hosts" -Encoding ascii -Force
