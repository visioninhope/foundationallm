#!/usr/bin/env pwsh

<#
.SYNOPSIS
    This script resizes the vectorization worker by updating the number of replicas.

.DESCRIPTION
    The script retrieves the vectorization worker name and resource group from azd environment variables.
    It then updates the vectorization worker by copying the container app revision with the specified number of replicas.

.PARAMETER size
    The number of replicas to set for the vectorization worker.

.EXAMPLE
    Resize-VectorizationWorker -size 3
    This example resizes the vectorization worker to have 3 replicas.

#>

Param(
    [Parameter(Mandatory = $true)][int]$size
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

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
    az containerapp revision copy `
        --name $serviceName `
        --resource-group $resourceGroup `
        --min-replicas $size `
        --max-replicas $size
}