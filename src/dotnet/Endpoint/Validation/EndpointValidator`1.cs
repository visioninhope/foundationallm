using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;

namespace FoundationaLLM.Endpoint.Validation
{
    /// <summary>
    /// Base validator for endpoint resource providers.
    /// </summary>
    /// <typeparam name="T">The type of endpoint to validate.</typeparam>
    public class EndpointValidator<T> : AbstractValidator<T> where T : EndpointBase
    {
        /// <summary>
        /// Validates the value of a specified configuration references for an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint object being validated.</param>
        /// <param name="configurationKey">The name of the configuration reference being validated.</param>
        /// <returns>True if the value of the configuration reference is valid. Otherwise, False.</returns>
        protected bool ValidateConfigurationReferences(EndpointBase endpoint, string configurationKey) =>
            endpoint.ConfigurationReferences!.ContainsKey(configurationKey)
                && !string.IsNullOrWhiteSpace(endpoint.ConfigurationReferences[configurationKey])
                && (string.Compare(
                    $"FoundationaLLM:Endpoints:{endpoint.Name}:{configurationKey}",
                    endpoint.ConfigurationReferences![configurationKey]) == 0);
    }
}
