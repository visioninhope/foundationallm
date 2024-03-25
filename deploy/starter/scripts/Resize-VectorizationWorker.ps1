#!/usr/bin/env pwsh

Param(
    [Parameter(Mandatory = $true)][int]$size
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

#!/usr/bin/env pwsh

function Invoke-AndRequireSuccess {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string]$Message,

        [Parameter(Mandatory = $true, Position = 1)]
        [ScriptBlock]$ScriptBlock
    )

    Write-Host "${message}..." -ForegroundColor Blue
    $result = & $ScriptBlock

    if ($LASTEXITCODE -ne 0) {
        throw "Failed ${message} (code: ${LASTEXITCODE})"
    }

    return $result
}

function Get-AzdEnvironmentVariable {
    param (
        [Parameter(Mandatory = $true)]$name
    )

    $result = azd env get-values | `
        Where-Object { $_.StartsWith($name) } | `
        ForEach-Object { $_.Split("=")[1].Trim('"') }

    return $result
}

$serviceName = Invoke-AndRequireSuccess "Get the vectorization worker name" {
    Get-AzdEnvironmentVariable -name "SERVICE_VECTORIZATION_JOB_NAME"
}

$resourceGroup = Invoke-AndRequireSuccess "Get the resource group" {
    Get-AzdEnvironmentVariable -name "RESOURCE_GROUP_NAME_DEFAULT"
}

Invoke-AndRequireSuccess "Update the vectorization worker" {
    az container app update `
        --name $serviceName `
        --resource-group $resourceGroup `
        --replicas $size
}
