using FoundationaLLM.Configuration.Catalog;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Configuration.Validation
{
    /// <summary>
    /// Provides health checks for the application's configuration settings.
    /// </summary>
    public static class ConfigurationHealthChecks
    {
        /// <summary>
        /// Uses the AppConfigCatalog to get the required configuration settings for the current version of the application.
        /// Any required settings that are missing will be returned in a list.
        /// </summary>
        /// <param name="configuration">The configuration object configured in the calling application, typically an Azure App Config instance.</param>
        /// <param name="currentVersion">The caller's current version.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetMissingRequiredConfigs(IConfiguration configuration, string currentVersion)
        {
            var missingKeys = new List<string>();

            var requiredEntries = AppConfigCatalog.GetRequiredConfigsForVersion(currentVersion);

            foreach (var entry in requiredEntries)
            {
                // Check if the configuration setting exists and has a value.
                var value = configuration[entry.Key];
                if (string.IsNullOrWhiteSpace(value))
                {
                    missingKeys.Add(entry.Key);
                }
            }

            return missingKeys;
        }
    }

}
