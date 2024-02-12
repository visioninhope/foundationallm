using FoundationaLLM.Configuration.Catalog;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Services;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;

namespace FoundationaLLM.Configuration.Validation
{
    /// <summary>
    /// Provides health checks for the application's configuration settings.
    /// </summary>
    public class ConfigurationHealthChecks(
        IAzureAppConfigurationService azureAppConfigurationService,
        IAzureKeyVaultService azureKeyVaultService,
        ILogger<ConfigurationHealthChecks> logger)
    {
        private readonly IAzureAppConfigurationService _azureAppConfigurationService = azureAppConfigurationService;
        private readonly IAzureKeyVaultService _azureKeyVaultService = azureKeyVaultService;
        private readonly ILogger<ConfigurationHealthChecks> _logger = logger;

        /// <summary>
        /// Validates the application's configuration settings.
        /// </summary>
        /// <param name="version">The current app version.</param>
        /// <returns></returns>
        public async Task ValidateConfigurationsAsync(string version)
        {
            var requiredEntries = AppConfigurationCatalog.GetRequiredConfigurationsForVersion(version);

            // Fetch existence maps for App Configuration settings and Key Vault secrets.
            var configExistenceMap = await _azureAppConfigurationService.CheckAppConfigurationSettingsExistAsync();
            var appConfigurationEntries = requiredEntries.ToList();
            var secretExistenceMap = await _azureKeyVaultService.CheckKeyVaultSecretsExistAsync(appConfigurationEntries
                .Where(entry => !string.IsNullOrEmpty(entry.KeyVaultSecretName))
                .Select(entry => entry.KeyVaultSecretName)
                .Distinct());

            // Identify missing App Configuration settings by checking the configExistenceMap.
            var missingConfigurations = appConfigurationEntries
                .Where(entry => !configExistenceMap.TryGetValue(entry.Key, out var configExists) || !configExists)
                .Select(entry => entry.Key);

            // Identify missing Key Vault secrets by checking the secretExistenceMap.
            var missingKeyVaultSecrets = appConfigurationEntries
                .Where(entry => !string.IsNullOrEmpty(entry.KeyVaultSecretName) &&
                                (!secretExistenceMap.TryGetValue(entry.KeyVaultSecretName, out var secretExists) || !secretExists))
                .Select(entry => entry.KeyVaultSecretName);

            // Prepare lists of missing configurations and secrets for logging and exception handling.
            var configurations = missingConfigurations.ToList();
            var keyVaultSecrets = missingKeyVaultSecrets.ToList();

            if (configurations.Count != 0 || keyVaultSecrets.Count != 0)
            {
                var errorMessage = $"Missing or empty app configurations: {string.Join(", ", configurations)}" +
                                   $"{(configurations.Count != 0 && keyVaultSecrets.Count != 0 ? "\n" : "")}" +
                                   $"Missing or empty Key Vault secrets for app configurations: {string.Join(", ", keyVaultSecrets)}";
                _logger.LogError(errorMessage);

                throw new ConfigurationValidationException(configurations, keyVaultSecrets);
            }
        }

        /// <summary>
        /// Validates the application's Key Vault secrets.
        /// </summary>
        /// <param name="version">The current app version.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationValidationException"></exception>
        public async Task ValidateKeyVaultSecretsAsync(string version)
        {
            var requiredSecrets = KeyVaultSecretsCatalog.GetRequiredKeyVaultSecretsForVersion(version);
            var keyVaultSecretEntries = requiredSecrets.ToList();
            var secretExistenceMap = await _azureKeyVaultService.CheckKeyVaultSecretsExistAsync(keyVaultSecretEntries
                .Select(entry => entry.SecretName));

            var missingSecrets = keyVaultSecretEntries
                .Where(entry => !secretExistenceMap.TryGetValue(entry.SecretName, out var secretExists) || !secretExists)
                .Select(entry => entry.SecretName);

            var missingKeyVaultSecrets = missingSecrets.ToList();
            if (missingKeyVaultSecrets.Count != 0)
            {
                var errorMessage = $"Missing or empty Key Vault secrets: {string.Join(", ", missingKeyVaultSecrets)}";
                _logger.LogError(errorMessage);
                throw new ConfigurationValidationException(new List<string>(), missingKeyVaultSecrets);
            }
        }
    }
}
