using FluentValidation;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Validation.Resources
{
    /// <summary>
    /// Validator for the <see cref="VectorizationRequest"/> model.
    /// </summary>
    public class VectorizationRequestValidator : AbstractValidator<VectorizationRequest>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="VectorizationRequest"/> model.
        /// </summary>
        public VectorizationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The vectorization request ID is required.");
                        
            // Optionally validate ObjectId if there are specific criteria it needs to meet, such as format.
            RuleFor(x => x.ObjectId)
                .NotEmpty().When(x => x.ObjectId != null)
                .WithMessage("The object ID must not be empty if provided.");

            RuleFor(x => x.ContentIdentifier)
                .Must(contentIdentifier =>
                    (!string.IsNullOrWhiteSpace(contentIdentifier?.UniqueId) ||
                    !string.IsNullOrWhiteSpace(contentIdentifier?.CanonicalId)) &&
                    contentIdentifier != null)
                .WithMessage("The vectorization request content identifier is invalid.");
           
            RuleFor(x => x.Steps)
                .Must(steps => steps.Select(step => step.Id).Distinct().Count() == steps.Count)
                .When(steps => steps != null) // This condition ensures that we don't evaluate the Must if Steps is null
                .WithMessage("The list of vectorization steps must contain unique names.");
        }
    }
}
