using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;

namespace FoundationaLLM.Endpoint.Validation
{
    /// <summary>
    /// Validator for the <see cref="AzureAIEndpoint"/> model.
    /// </summary>
    public class AzureAIEndpointValidator : EndpointValidator<AzureAIEndpoint>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="AzureAIEndpoint"/> model.
        /// </summary>
        public AzureAIEndpointValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            Include(new EndpointBaseValidator());

            RuleFor(x => x)
                .Must(e => ValidateConfigurationReferences(e, "endpoint"))
                .WithMessage("The endpoint configuration reference is missing or has an invalid value.");

            RuleFor(x => x)
                .Must(e => ValidateConfigurationReferences(e, "api_version"))
                .WithMessage("The api_version configuration reference is missing or has an invalid value.");

            RuleFor(x => x)
                .Must(e => ValidateConfigurationReferences(e, "auth_type"))
                .WithMessage("The auth_type configuration reference is missing or has an invalid value.");

            RuleFor(x => ValidateConfigurationReferences(x, "api_key"))
                .NotEmpty().When(e => e.ConfigurationReferences!.GetValueOrDefault("auth_type") == "key")
                .WithMessage("The api_key configuration reference is missing or has an invalid value.");
        }
    }
}
