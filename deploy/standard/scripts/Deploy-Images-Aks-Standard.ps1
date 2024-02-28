#! /usr/bin/pwsh

<#
.SYNOPSIS
    Deploy-Images-Aks-Standard.ps1 is a script used to deploy images to Azure Kubernetes Service (AKS) clusters.

.DESCRIPTION
    This script deploys images to both the frontend and backend AKS clusters. It retrieves the credentials for the AKS clusters,
    then calls the Deploy-Backend-Images-Aks.ps1 and Deploy-Frontend-Images-Aks.ps1 scripts to deploy the backend and frontend images, respectively.

.PARAMETER name
    The name of the deployment. Default value is "foundationallm".

.PARAMETER frontendAksName
    The name of the frontend AKS cluster. This parameter is mandatory.

.PARAMETER backendAksName
    The name of the backend AKS cluster. This parameter is mandatory.

.PARAMETER resourceGroup
    The name of the resource group containing the AKS clusters. This parameter is mandatory.

.PARAMETER charts
    The charts to deploy. Default value is "*".

.PARAMETER namespace
    The namespace to deploy the images to. Default value is "fllm".

.PARAMETER frontendHostname
    The hostname for the frontend. Default value is "www.internal.foundationallm.ai".

.PARAMETER backendHostname
    The hostname for the backend. Default value is "api.internal.foundationallm.ai".

.PARAMETER autoscale
    Specifies whether to enable autoscaling. Default value is $false.

.PARAMETER version
    The version of the images to deploy. Default value is "0.4.1".

.EXAMPLE
    Deploy-Images-Aks-Standard.ps1 -frontendAksName "frontend-aks" -backendAksName "backend-aks" -resourceGroup "my-resource-group"

    This example deploys images to the "frontend-aks" and "backend-aks" AKS clusters in the "my-resource-group" resource group.

#>

Param(
    [parameter(Mandatory = $false)][string]$name = "foundationallm",
    [parameter(Mandatory = $true)][string]$frontendAksName,
    [parameter(Mandatory = $true)][string]$backendAksName,
    [parameter(Mandatory = $true)][string]$resourceGroup,
    [parameter(Mandatory = $false)][string]$charts = "*",
    [parameter(Mandatory = $false)][string]$namespace = "fllm",
    [parameter(Mandatory = $false)][string]$frontendHostname = "www.internal.foundationallm.ai",
    [parameter(Mandatory = $false)][string]$backendHostname = "api.internal.foundationallm.ai",
    [parameter(Mandatory = $false)][bool]$autoscale = $false,
    [parameter(Mandatory = $false)][string]$version = "0.4.1"
)

Push-Location $($MyInvocation.InvocationName | Split-Path)

az aks get-credentials `
    -n $backendAksName `
    -g $resourceGroup

& ./Deploy-Backend-Images-Aks.ps1 `
    -name $name `
    -aksName $backendAksName `
    -resourceGroup $resourceGroup `
    -namespace $namespace `
    -charts $charts `
    -version $version

Pop-Location

Push-Location $($MyInvocation.InvocationName | Split-Path)

az aks get-credentials `
    -n $frontendAksName `
    -g $resourceGroup

& ./Deploy-Frontend-Images-Aks.ps1 `
    -name $name `
    -aksName $frontendAksName `
    -resourceGroup $resourceGroup `
    -namespace $namespace `
    -charts $charts `
    -version $version

Pop-Location
