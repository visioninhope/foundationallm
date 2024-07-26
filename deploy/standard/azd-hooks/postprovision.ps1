#!/usr/bin/env pwsh

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

# Load utility functions
Push-Location $($MyInvocation.InvocationName | Split-Path)
try {
    . ./utility/Load-Utility-Functions.ps1
}
finally {
    Pop-Location
}

# Navigate to the script directory so that we can use relative paths.
Push-Location $($MyInvocation.InvocationName | Split-Path)
try {
    # Convert the manifest resource groups to a hashtable for easier access
    $resourceGroup = @{
        app     = $env:FLLM_APP_RG
        auth    = $env:FLLM_AUTH_RG
        data    = $env:FLLM_DATA_RG
        dns     = $env:FLLM_DNS_RG
        jbx     = $env:FLLM_JBX_RG
        net     = $env:FLLM_NET_RG
        oai     = $env:FLLM_OAI_RG
        ops     = $env:FLLM_OPS_RG
        storage = $env:FLLM_STORAGE_RG
        vec     = $env:FLLM_VEC_RG
    }

    Invoke-AndRequireSuccess "Generate Host File" {
        ./utility/Generate-Hosts.ps1 `
            -resourceGroup $resourceGroup `
            -subscription $env:AZURE_SUBSCRIPTION_ID
    }
}
finally {
    Pop-Location
    Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
}
