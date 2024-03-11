using FluentValidation;
using FoundationaLLM.Authorization.Models;

namespace FoundationaLLM.Authorization.Validation
{
    /// <summary>
    /// Validator for the <see cref="ActionAuthorizationRequest"/> model.
    /// </summary>
    public class ActionAuthorizationRequestValidator : AbstractValidator<ActionAuthorizationRequest>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="ActionAuthorizationRequest"/> model.
        /// </summary>
        public ActionAuthorizationRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.InstanceId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The FoundationaLLM instance identifier must be a valid string.");

            RuleFor(x => x.Action)
                .NotNull()
                .NotEmpty()
                .WithMessage("The action must be a valid string.");

            RuleFor(x => x.ResourcePath)
                .NotNull()
                .NotEmpty()
                .WithMessage("The resource must be a valid string.");

            RuleFor(x => x.PrincipalId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The principal identifier must be a valid string.");
        }
    }
}
