<#
.SYNOPSIS
    Creates Azure Private DNS Zones and links them to specified Virtual Networks (VNets).

.DESCRIPTION
    This script automates the creation of Azure Private DNS Zones and links them to a set of hardcoded Virtual Networks (VNets). 
    The script accepts the resource group and location as parameters, while the DNS zones and VNets are predefined in the script.

    Prerequisites:
    - Azure CLI must be installed and configured.
    - The user must be logged in using `az login`.
    - The specified resource group and VNets must already exist in Azure.

.PARAMETER ResourceGroup
    The name of the Azure resource group where the Private DNS Zones and VNets are located. This is a required parameter.

.PARAMETER Location
    The Azure region where the Private DNS Zones will be created. This is a required parameter.

.EXAMPLE
    ./Deploy-FllmPrivateDns.ps1 -ResourceGroup "YourResourceGroupName"
    This example shows how to run the script to create the Private DNS Zones and link them to the VNets in the specified resource group.

.NOTES
    This helper script can be used to create the Private DNS zones used by FLLM in a specific resource group and link them to the VNets.
#>

param (
	[Parameter(Mandatory = $true)]
	[string] $resourceGroupName,
	[Parameter(Mandatory = $true)]
	[string] $location
)

# Set Debugging and Error Handling
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

# Define the arrays (hardcoded)
$PrivateZones = @(
	"privatelink.monitor.azure.com",
	"privatelink.blob.core.windows.net",
	"privatelink.dfs.core.windows.net",
	"privatelink.documents.azure.com",
	"privatelink.$location.azmk8s.io",
	"privatelink.vaultcore.azure.net",
	"privatelink.agentsvc.azure-automation.net",
	"privatelink.azconfig.io",
	"privatelink.cognitiveservices.azure.com",
	"privatelink.file.core.windows.net",
	"privatelink.ods.opinsights.azure.com",
	"privatelink.oms.opinsights.azure.com",
	"privatelink.openai.azure.com",
	"privatelink.queue.core.windows.net",
	"privatelink.search.windows.net",
	"privatelink.table.core.windows.net"
)
$VNets = @(
	"vnet-fllm",
	"vnet-hub"
)

# Create Private DNS Zones and link them to VNets
foreach ($zone in $PrivateZones) {
	# Create the Private DNS Zone
	Write-Host -ForegroundColor Yellow "Creating Private DNS Zone: $zone"
	az network private-dns zone create --resource-group $resourceGroupName --name $zone

	foreach ($vnet in $VNets) {
		# Get the VNet ID
		$vnetId = az network vnet show --resource-group $resourceGroupName --name $vnet --query "id" --output tsv

		# Create a Virtual Network Link
		Write-Host -ForegroundColor Yellow "Linking VNet: $vnet to Zone: $zone"
		$linkName = "$($vnet)-$($zone)-link"
		az network private-dns link vnet create --resource-group $resourceGroupName --zone-name $zone --name $linkName --virtual-network $vnetId --registration-enabled false
	}
}

Write-Host -ForegroundColor Green "Private DNS Zones and VNet Links have been created successfully."