#! /usr/bin/pwsh

Param(
    [parameter(Mandatory = $false)][bool]$init = $true,
    [parameter(Mandatory = $false)][string]$loginServer = "ghcr.io/solliancenet/foundationallm",
    [parameter(Mandatory = $false)][string]$manifestName = "Deployment-Manifest.json",
    [parameter(Mandatory = $false)][string]$updateVersion = "0.4.1"

)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

# Load the Invoke-AndRequireSuccess function
. ./utility/Invoke-AndRequireSuccess.ps1

# Navigate to the script directory so that we can use relative paths.
Push-Location $($MyInvocation.InvocationName | Split-Path)
try {
    Write-Host "Loading Deployment Manifest ../${manifestName}" -ForegroundColor Blue
    $manifest = $(Get-Content -Raw -Path ../${manifestName} | ConvertFrom-Json)

    $resourceGroup = @{}
    $manifest.resourceGroups.PSObject.Properties | ForEach-Object { $resourceGroup[$_.Name] = $_.Value }

    $backendAks = Invoke-AndRequireSuccess "Get Backend AKS" {
        az aks list `
            --resource-group $($resourceGroup.app) `
            --query "[?contains(name, 'backend')].name | [0]" `
            --output tsv
    }

    $chartNames = @{
        "agent-factory-api"          = "../config/helm/microservice-values.yml"
        "agent-hub-api"              = "../config/helm/microservice-values.yml"
        "core-api"                   = "../config/helm/coreapi-values.yml"
        "core-job"                   = "../config/helm/microservice-values.yml"
        "data-source-hub-api"        = "../config/helm/microservice-values.yml"
        "gatekeeper-api"             = "../config/helm/microservice-values.yml"
        "gatekeeper-integration-api" = "../config/helm/microservice-values.yml"
        "langchain-api"              = "../config/helm/microservice-values.yml"
        "management-api"             = "../config/helm/managementapi-values.yml"
        "prompt-hub-api"             = "../config/helm/microservice-values.yml"
        "semantic-kernel-api"        = "../config/helm/microservice-values.yml"
        "vectorization-api"          = "../config/helm/vectorizationapi-values.yml"
        "vectorization-job"          = "../config/helm/microservice-values.yml"
    }
    Invoke-AndRequireSuccess "Update Backend" {
        ./deploy/Update-Aks.ps1 `
            -aksName $backendAks `
            -chartNames $chartNames `
            -loginServer $loginServer `
            -resourceGroup $resourceGroup.app `
            -updateVersion $updateVersion
    }

    $frontendAks = Invoke-AndRequireSuccess "Get Frontend AKS" {
        az aks list `
            --resource-group $($resourceGroup.app) `
            --query "[?contains(name, 'frontend')].name | [0]" `
            --output tsv
    }

    $chartNames = @{
        "chat-ui"       = "../config/helm/chatui-values.yml"
        "management-ui" = "../config/helm/managementui-values.yml"
    }
    Invoke-AndRequireSuccess "Update Frontend" {
        ./deploy/Update-Aks.ps1 `
            -aksName $frontendAks `
            -chartNames $chartNames `
            -loginServer $loginServer `
            -resourceGroup $resourceGroup.app `
            -updateVersion $updateVersion
    }
}
finally {
    Pop-Location
    Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
}
