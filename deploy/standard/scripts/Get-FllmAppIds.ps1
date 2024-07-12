# Define the list of app registration names
$appNames = @("FoundationaLLM", "FoundationaLLM-Authorization", "FoundationaLLM-Client", "FoundationaLLM-Management", "FoundationaLLM-Management-Client")  # Add other app names as needed

# Loop through each app name and get the app ID
foreach ($appName in $appNames) {
	$appId = az ad app list --display-name $appName --query "[?displayName=='$appName'].appId" -o tsv
	Write-Output "$appName ID: $appId"
}
