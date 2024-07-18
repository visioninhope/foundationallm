param(
    [Parameter(Mandatory = $true)][string]$kvName,
    [Parameter(Mandatory = $true)][string]$coreApiUrl,
    [Parameter(Mandatory = $true)][string]$loadTestResourceName,
    [Parameter(Mandatory = $true)][string]$loadTestResourceGroup,
    [Parameter(Mandatory = $true)][string]$subnetId
)

$token = $(az account get-access-token --scope "api://FoundationaLLM-Auth/.default" --query accessToken -o tsv)
$kvUrl = $(az keyvault secret set --vault-name $kvName --name "bearer-token" --value $token --query '{id: id}' | ConvertFrom-Json).id

$load_test_name = "FLLM-Load-Test-$([int64](Get-Date -UFormat %s))"
$load_test_file_name = "Load-Test-Questions.csv"
$load_test_id = (New-Guid).Guid

$config = @{
    load_test_name = $load_test_name
    load_test_id = $load_test_id
    load_test_file_name = $load_test_file_name
    token = $kvUrl
    core_api_endpoint = $coreApiUrl
    subnet_id = $subnetId
}
$load_testing_config = Get-Content "./load-testing-template-standard.yml" -Raw
foreach ($h in $config.GetEnumerator())
{
    $load_testing_config = $load_testing_config.replace("`${$($h.Name)}", $h.Value)
}
Out-File -FilePath "load-testing.yml" -InputObject $load_testing_config

az load test create `
    --load-test-resource $loadTestResourceName `
    --test-id $load_test_id `
    --load-test-config-file "load-testing.yml" `
    -g $loadTestResourceGroup

az load test-run create `
    --load-test-resource $loadTestResourceName `
    --test-id $load_test_id `
    --test-run-id $(New-Guid).Guid `
    -g $loadTestResourceGroup