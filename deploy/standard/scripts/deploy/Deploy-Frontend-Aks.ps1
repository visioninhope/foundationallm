#! /usr/bin/pwsh

Param (
    [parameter(Mandatory = $true)][object]$instanceId,
    [parameter(Mandatory = $true)][object]$entraClientIds,
    [parameter(Mandatory = $true)][object]$resourceGroups,
    [parameter(Mandatory = $true)][string]$resourceSuffix,
    [parameter(Mandatory = $true)][string]$subscriptionId,
    [parameter(Mandatory = $true)][object]$ingress
)

Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"