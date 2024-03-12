#! /usr/bin/env pwsh

param(
    [parameter(Mandatory = $true)][string]$keyVaultResourceGroup
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)

. ./utility/Invoke-AndRequireSuccess.ps1

$directories = @{
    "config" = "../config/certbot/config"
    "certs"  = "../config/certbot/certs"
}

$keyVaultName = Invoke-AndRequireSuccess "Retrieve Key Vault" {
    az keyvault list `
        --resource-group $keyVaultResourceGroup `
        --query "[0].name" `
        --output tsv
}

foreach ($pfx in Get-ChildItem -Path $directories["certs"] -Filter "*.pfx") {
    $domain = $pfx.BaseName
    $keyName = $domain -replace "\.", "-"

    Invoke-AndRequireSuccess "Load PFX Certificate $($domain) into Azure Key Vault" {
        az keyvault certificate import `
            --file $pfx.FullName `
            --name $keyName `
            --vault-name $keyVaultName
    }

    $certificate = Join-Path $directories["config"] "live" ${domain} "fullchain.pem" | Resolve-Path
    $certificateSecretName = @($keyName, "cert") | Join-String -Separator "-"
    Invoke-AndRequireSuccess "Load PEM Certificate $($domain) into Azure Key Vault" {
        az keyvault secret set `
            --file $certificate `
            --name $certificateSecretName `
            --vault-name $keyVaultName | `
            Out-Null
    }

    $certificateChain = Join-Path $directories["config"] "live" ${domain} "chain.pem" | Resolve-Path
    $certificateChainSecretName = @($keyName, "ca") | Join-String -Separator "-"
    Invoke-AndRequireSuccess "Load PEM Certificate Chain $($domain) into Azure Key Vault" {
        az keyvault secret set `
            --file $certificateChain `
            --name $certificateChainSecretName `
            --vault-name $keyVaultName | `
            Out-Null
    }

    $privateKey = Join-Path $directories["config"] "live" ${domain} "privkey.pem" | Resolve-Path
    $privateKeySecretName = @($keyName, "key") | Join-String -Separator "-"
    Invoke-AndRequireSuccess "Load Private Key $($domain) into Azure Key Vault" {
        az keyvault secret set `
            --file $privateKey `
            --name $privateKeySecretName `
            --vault-name $keyVaultName | `
            Out-Null
    }
}
