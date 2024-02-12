using FoundationaLLM.Common.Services;
using FoundationaLLM.Configuration.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FoundationaLLM.Common.Constants;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM
{
    /// <summary>
    /// Provides extension methods used to configure dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Registers configuration health checks and related services.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration to bind settings from.</param>
        public static void AddConfigurationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Azure App Configuration Service.
            services.AddSingleton<IAzureAppConfigurationService>(sp =>
            {
                var configClient = new ConfigurationClient(configuration[EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString]);
                var logger = sp.GetRequiredService<ILogger<AzureAppConfigurationService>>();
                return new AzureAppConfigurationService(configClient, logger);
            });

            // Register Azure Key Vault Service.
            services.AddSingleton<IAzureKeyVaultService>(sp =>
            {
                var keyVaultUri = configuration[AppConfigurationKeys.FoundationaLLM_Configuration_KeyVaultURI];
                var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
                return new AzureKeyVaultService(secretClient);
            });

            // Register Configuration Health Checks Service.
            services.AddSingleton<ConfigurationHealthChecks>(sp =>
            {
                var appConfigService = sp.GetRequiredService<IAzureAppConfigurationService>();
                var keyVaultService = sp.GetRequiredService<IAzureKeyVaultService>();
                var logger = sp.GetRequiredService<ILogger<ConfigurationHealthChecks>>();
                return new ConfigurationHealthChecks(appConfigService, keyVaultService, logger);
            });
        }
    }
}
