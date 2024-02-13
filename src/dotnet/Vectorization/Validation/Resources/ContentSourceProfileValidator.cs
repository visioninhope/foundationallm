using FluentValidation;
using FoundationaLLM.Vectorization.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Validation.Resources
{
    /// <summary>
    /// Validator for the <see cref="ContentSourceProfile"/> model.
    /// </summary>
    public class ContentSourceProfileValidator : AbstractValidator<ContentSourceProfile>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="ContentSourceProfile"/> model.
        /// </summary>
        public ContentSourceProfileValidator()
        {
            // Include the base validator to apply its rules
            Include(new VectorizationProfileBaseValidator());

            // Validate the Type property to ensure it's a valid enum value.
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("The content source type must be a valid value.");
        }
    }
}
