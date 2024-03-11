#! /usr/bin/env pwsh

param(
    [parameter(Mandatory = $true)][string]$baseDomain,
    [parameter(Mandatory = $true)][string]$email,
    [parameter(Mandatory = $false)][string]$subdomainPrefix=""
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"
Set-PSDebug -Trace 0 # Echo every command (0 to disable, 1 to enable, 2 to enable verbose)

. ./utility/Invoke-AndRequireSuccess.ps1

$basenames = @(
    "api"
    "management"
    "management-api"
    "vectorization-api"
    "www"
)

$directories = @{
    "config" = "./certbot/config"
    "work"   = "./certbot/work"
    "log"    = "./certbot/log"
    "certs"  = "../config/certs"
}

foreach ($directory in $directories.GetEnumerator()) {
    if (!(Test-Path $directory.Value)) {
        New-Item -ItemType Directory -Force -Path $directory.Value
    }
}

foreach ($basename in $basenames) {
    # Domain Name
    $hostname = @($subdomainPrefix, $basename) | Join-String -Separator "-"
    $fqdn = @($hostname, $baseDomain) | Join-String -Separator "."

    # File Paths
    $paths = @{
        "pemFullChain" = Join-Path $directories["config"] "live" $fqdn "fullchain.pem"
        "pemPrivKey" = Join-Path $directories["config"] "live" $fqdn "privkey.pem"
        "pfx" = Join-Path $directories["certs"] "${fqdn}.pfx"
    }

    Invoke-AndRequireSuccess "Generate certificate for ${fqdn}" {
        certbot certonly `
            --agree-tos `
            --email $email `
            --authenticator dns-azure `
            --config-dir $directories["config"] `
            --dns-azure-config certbot.ini `
            --domain $fqdn `
            --keep-until-expiring `
            --logs-dir $directories["log"] `
            --non-interactive `
            --preferred-challenges dns `
            --quiet `
            --work-dir $directories["work"]
    }

    Invoke-AndRequireSuccess "Export certificate for ${fqdn}" {
        openssl pkcs12 `
            -export `
            -inkey $paths["pemPrivKey"] `
            -in $paths["pemFullChain"] `
            -out $paths["pfx"] `
            -passout pass:
    }

    Invoke-AndRequireSuccess "Verify certificate for ${fqdn}" {
        openssl pkcs12 `
            -info `
            -in $paths["pfx"] `
            -nokeys `
            -passin pass: `
            -passout pass:
    }
}
