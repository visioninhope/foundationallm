#! /usr/bin/env pwsh

param(
    [parameter(Mandatory = $true)][array]$certificates,
    [parameter(Mandatory = $true)][string]$keyVaultName,
    [parameter(Mandatory = $true)][string]$keyVaultResourceGroup
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)

. ./utility/Load-Utility-Functions.ps1

$directories = @{
    "certs"  = "../certs"
}

foreach ($certificate in $certificates) {
    $pfxPath = Join-Path $directories["certs"] "$certificate.pfx"
    $pfx = Get-ChildItem -Path $pfxPath
    $keyName = $certificate

    Invoke-AndRequireSuccess "Load PFX Certificate $($certificate) into Azure Key Vault" {
        az keyvault certificate import `
            --file $pfx.FullName `
            --name $keyName `
            --vault-name $keyVaultName
    }
}
