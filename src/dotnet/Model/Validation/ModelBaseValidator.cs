using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Model;
using FoundationaLLM.Common.Validation.ResourceProvider;

namespace FoundationaLLM.Model.Validation
{
    /// <summary>
    /// Validator for the <see cref="ModelBase"/> model.
    /// </summary>
    public class ModelBaseValidator : AbstractValidator<ModelBase>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="ModelBase"/> model.
        /// </summary>
        public ModelBaseValidator() =>
            Include(new ResourceBaseValidator());
    }
}
