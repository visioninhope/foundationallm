using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Validation.ResourceProvider;

namespace FoundationaLLM.Attachment.Validation
{
    /// <summary>
    /// Validator for the <see cref="AttachmentBase"/> model.
    /// </summary>
    public class AttachmentBaseValidator : AbstractValidator<AttachmentBase>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="AttachmentBase"/> model.
        /// </summary>
        public AttachmentBaseValidator() =>
            Include(new ResourceBaseValidator());
    }
}
