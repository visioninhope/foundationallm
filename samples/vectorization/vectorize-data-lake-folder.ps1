
$developmentEnvironment = $true
$subscriptioName = "FoundationaLLM Sandbox"
$keyVaultName = "fllmaks14-kv"
$appConfigName = "fllmaks14-appconfig"

$vectorizationAPIUrlConfigName = "FoundationaLLM:APIs:VectorizationAPI:APIUrl"
$vectorizationAPIKeySecretName = "foundationallm-apis-vectorizationapi-apikey"
# $vectorizationApplicationIdUri = "api://FoundationaLLM-Vectorization"

$vectorizationInputStorageAccountName = "fllmaks14sa"
$vectorizaitonInputContainerName = "vectorization-input"
$vectorizationInputFolderPath = "wtw"
$vectorizationCanonicalRootPath = "wtw"

$contentSourceProfile = "SDZWAJournals3"
$textPartitionProfile = "DefaultTokenTextPartition_Small_v3"
$textEmbeddingProfile = "AzureOpenAI_Embedding_v2"
$indexingProfile = "AzureAISearch_Default_003"

az login
az account set --subscription $subscriptioName

# $accessToken = (az account get-access-token --resource $vectorizationApplicationIdUri --query accessToken -o tsv)

if ($developmentEnvironment) {
    $vectorizationAPIUrl = "https://localhost:7047"
} else {
    $vectorizationAPIUrl = (az appconfig kv show --name $appConfigName --key $vectorizationAPIUrlConfigName --query value -o tsv)
}
$vectorizationAPIKey = (az keyvault secret show --name $vectorizationAPIKeySecretName --vault-name $keyVaultName --query value -o tsv)


$filesToVectorize = (az storage fs file list `
    -f $vectorizaitonInputContainerName `
    --path $vectorizationInputFolderPath `
    --account-name $vectorizationInputStorageAccountName `
    --auth-mode login `
    --query [].name -o tsv)

foreach ($filePath in $filesToVectorize) {
    
    $fileName = Split-Path -Path $filePath -Leaf
    $fileNameNoExtension = Split-Path -Path $filePath -LeafBase
    write-host "Vectorizing file: $fileName..."

    
    write-host $vectorizationRequest
}

$vectorizationRequest = @"
    {
        "content_identifier": {
            "content_source_profile_name": $contentSourceProfile,
            "multipart_id": [
                "$($vectorizationInputStorageAccountName).dfs.core.windows.net",
                $vectorizaitonInputContainerName,
                "$($vectorizationInputFolderPath)/$($fileName)"
            ],
            "canonical_id": "$($vectorizationCanonicalRootPath)/$($fileNameNoExtension)"
        },
        "processing_type": "Asynchronous",
        "steps":[
            {
                "id": "extract",
                "parameters": {
                }
            },
            {
                "id": "partition",
                "parameters": {
                    "text_partition_profile_name": $textPartitionProfile
                }
            },
            {
                "id": "embed",
                "parameters": {
                    "text_embedding_profile_name": $textEmbeddingProfile
                }
            },
            {
                "id": "index",
                "parameters": {
                    "indexing_profile_name": $indexingProfile
                }
            }
        ]
    }
