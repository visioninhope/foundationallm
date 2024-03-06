using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Configuration.Environment;

namespace FoundationaLLM.Configuration.Catalog
{
    /// <summary>
    /// Provides the catalog of environment variables required by the solution.
    /// </summary>
    public static class EnvironmentVariablesCatalog
    {
        private static readonly List<EnvironmentVariableEntry> GenericEntries =
            [
                new EnvironmentVariableEntry(EnvironmentVariables.Hostname,
                    "The Azure Container App or Azure Kubernetes Service hostname."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_Version,
                    "The build version of the container. This is also used for the app version used to validate the minimum version of the app required to use certain configuration entries."),
            ];

        private static readonly List<EnvironmentVariableEntry> AppConfigEntries =
            [
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString,
                    "The connection string for the App Configuration service.")
            ];

        private static readonly List<EnvironmentVariableEntry> AuthorizationEntries =
            [
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_AppInsightsConnectionString,
                    "The connection string used by OpenTelemetry to connect to App Insights."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_Instance,
                    "The Entra ID instance."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_TenantId,
                    "The Entra ID tenant id."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_ClientId,
                    "The Entra ID client id."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_ClientSecret,
                    "The Entra ID client secret."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_Scopes,
                    "The Entra ID scopes."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Storage_AccountName,
                    "The name of the storage account used by the Authorization API."),
                new EnvironmentVariableEntry(EnvironmentVariables.FoundationaLLM_AuthorizationAPI_InstanceIds,
                    "The comma separated list of the identifiers of FoundationaLLM instances managed by the authorization core."),
            ];

        /// <summary>
        /// Returns the required environment variables.
        /// </summary>
        /// <param name="serviceName">Optional service name. When not specified, the default environment variables list is returned.</param>
        /// <returns></returns>
        public static IEnumerable<EnvironmentVariableEntry> GetRequiredEnvironmentVariables(
            string serviceName = "") =>
            serviceName switch
            {
                ServiceNames.AuthorizationAPI => GenericEntries.Concat(AuthorizationEntries),
                _ => GenericEntries.Concat(AuthorizationEntries),
            };
    }

}
