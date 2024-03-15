#! /usr/bin/pwsh

Param(
    # [parameter(Mandatory=$false)][string]$name = "foundationallm",
    [parameter(Mandatory = $false)][string]$aksName,
    [parameter(Mandatory = $false)][string]$resourceGroup,
    [parameter(Mandatory = $false)][string]$secretProviderClassManifest
    # [parameter(Mandatory=$false)][string]$charts = "*",
    # [parameter(Mandatory=$false)][string]$namespace = "fllm",
    # [parameter(Mandatory=$false)][bool]$autoscale=$false,
    # [parameter(Mandatory=$false)][string]$version="0.4.1"
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

    Write-Host "${message}..." -ForegroundColor Blue
    $result = & $ScriptBlock

    if ($LASTEXITCODE -ne 0) {
        throw "Failed ${message} (code: ${LASTEXITCODE})"
    }

    return $result
}

Invoke-AndRequireSuccess "Retrieving credentials for AKS cluster ${aksName}" {
    az aks get-credentials --name $aksName --resource-group $resourceGroup
}

$gatewayNamespace = "gateway-system"
$gatewayNamespaceYaml = @"
apiVersion: v1
kind: Namespace
metadata:
  name: ${gatewayNamespace}
"@
Write-Host $gatewayNamespaceYaml
Invoke-AndRequireSuccess "Create ${gatewayNamespace} namespace" {
    $gatewayNamespaceYaml | kubectl apply --filename -
}

Invoke-AndRequireSuccess "Deploying secret provider class" {
    kubectl apply `
        --filename=${secretProviderClassManifest} `
        --namespace=${gatewayNamespace}
}

# Deploy nginx