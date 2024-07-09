#!/usr/bin/env pwsh

Param(
    [Parameter(Mandatory = $true)][string]$resourceGroup
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

function Get-AbsolutePath {
    <#
    .SYNOPSIS
    Get the absolute path of a file or directory. Relative path does not need to exist.
    #>
    param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, Position = 0)]
        [string]$RelatviePath
    )

    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($RelatviePath)
}

function Invoke-CLICommand {
    <#
    .SYNOPSIS
    Invoke a CLI Command and allow all output to print to the terminal.  Does not check for return values or pass the output to the caller.
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string]$Message,

        [Parameter(Mandatory = $true, Position = 1)]
        [ScriptBlock]$ScriptBlock
    )

    Write-Host "${message}..." -ForegroundColor Blue
    & $ScriptBlock

    if ($LASTEXITCODE -ne 0) {
        throw "Failed ${message} (code: ${LASTEXITCODE})"
    }
}

# Navigate to the script directory so that we can use relative paths.
Push-Location $($MyInvocation.InvocationName | Split-Path)
try {
    $testDataPath = "../data/backup/$resourceGroup" | Get-AbsolutePath
    Write-Host "Writing environment files to: $testDataPath" -ForegroundColor Yellow
    # Create test data path if it doesn't already exist
    if (-not (Test-Path $testDataPath)) {
        New-Item -ItemType Directory -Path $testDataPath
    }

    # Check if container exists in the storage account
    az storage container create -n backups --account-name foundationallmdata

    # Get main storage account
    $numStorageAccounts = az storage account list -g $resourceGroup --query "length(@ )"
    if ($numStorageAccounts -eq 0) {
        throw "There are $numStorageAccounts storage accounts in $resourceGroup. Target a resource group with one storage account."
    }
    $sourceStorageAccountName = (az storage account list -g $resourceGroup --query "@[0].name").Trim('"')

    $env:AZCOPY_AUTO_LOGIN_TYPE="AZCLI"
    foreach ($container in ((az storage container list --account-name $sourceStorageAccountName --query "@[].name" --auth-mode login) | ConvertFrom-Json)) {
        azcopy copy "https://$($sourceStorageAccountName).blob.core.windows.net/$container/" "https://foundationallmdata.blob.core.windows.net/backups/$resourceGroup/" --recursive
    }
}
catch {
    $logFile = Get-ChildItem -Path "$env:HOME/.azcopy" -Filter "*.log" | `
        Where-Object { $_.Name -notlike "*-scanning*" } | `
        Sort-Object LastWriteTime -Descending | `
        Select-Object -First 1
    $logFileContent = Get-Content -Raw -Path $logFile.FullName
    Write-Host $logFileContent
}
finally {
    Pop-Location
    Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
}

# Copy the storage account contents
# Copy the indexes