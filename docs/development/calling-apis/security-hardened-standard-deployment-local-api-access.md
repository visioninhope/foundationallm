# Security-Hardened Standard Deployment Local API Access

Unlike starter deployments, security-hardened standard deployments expose backend services internally, preventing public API access. Using the Azure VPN Gateway Client and the `kubectl` CLI, however, it is possible to forward FoundationaLLM APIs deployed within Kubernetes for local consumption.

## Networking Requirements

- Use the Azure VPN Gateway Client to enable network access to the same virtual network used by the Kubernetes nodes and the managed Azure services
- If the Azure VPN Gateway Client is unable to resolve Private Link DNS names, you may need to modify your system's hosts file
  - The standard deployment scripts generate the hosts file entries automatically
  - After updating your system's hosts file, ensure that you refresh your DNS cache

## kubectl Forwarding Script

### Prerequisites

- `kubectl` with the [`kubelogin`](https://azure.github.io/kubelogin/) extension
  - Both of these utilities can be installed by the Azure CLI: `az aks install-cli`
- Kubernetes credentials stored in `$HOME/.kube/config`

  Obtain these credentials using the Azure CLI.

  ```
  az aks get-credentials --name MyManagedCluster --resource-group MyResourceGroup
  ```

### Script

Navigate to `/deploy/standard/scripts/Kubectl-Proxy.ps1` or copy the following PowerShell script to your local environment. Before running it, ensure that no applications are running on ports 5000-5010.

```pwsh
#!/bin/pwsh
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

$services = @{
    "foundationallm-agent-factory-api" = 5000
    "foundationallm-gatekeeper-api" = 5001
    "foundationallm-agent-hub-api" = 5002
    "foundationallm-core-api" = 5003
    "foundationallm-data-source-hub-api" = 5004
    "foundationallm-gatekeeper-integration-api" = 5005
    "foundationallm-langchain-api" = 5006
    "foundationallm-management-api" = 5007
    "foundationallm-prompt-hub-api" = 5008
    "foundationallm-semantic-kernel-api" = 5009
    "foundationallm-vectorization-api" = 5010
}
$jobIds = @()

try {
    foreach ($servicePortPairing in $services.GetEnumerator()) {
        Write-Host "Starting Kubectl Tunnel for $($servicePortPairing.key)"
        $job = Start-Job -ScriptBlock ([scriptblock]::Create("kubectl port-forward service/$($servicePortPairing.key) $($servicePortPairing.value):80"))
        Write-Host "Job: $($job.Command)"
        $jobIds += $job.Id
    }

    Write-Host "Press any key to kill the Kubernetes tunnels..."
    $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
catch {}
finally {
    foreach ($jobId in $jobIds) {
        Write-Host "Killing $jobId"
        Stop-Job -Id $jobId
    }
}
```

### Verification

Run the following script to ensure that all APIs are accessible through `kubectl` forwarding.

```pwsh
#!/bin/pwsh
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

foreach ($servicePort in 5000..5010) {
    Write-Host "Testing Port #$servicePort..."
    curl "http://localhost:$servicePort/status"
}
```
