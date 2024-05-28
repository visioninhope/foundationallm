using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;

namespace FoundationaLLM.Attachment.Validation
{
    /// <summary>
    /// Validator for the <see cref="AudioAttachment"/> model.
    /// </summary>
    public class AudioAttachmentValidator : AttachmentValidator<AudioAttachment>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="AudioAttachment"/> model.
        /// </summary>
        public AudioAttachmentValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            Include(new AttachmentBaseValidator());

            RuleFor(x => x.Path)
                .NotNull()
                .NotEmpty()
                .WithMessage("The attachment path must contain a value.");

            RuleFor(x => x.Content)
                .NotNull()
                .Must(stream => stream != null && stream.CanRead && stream.Length > 0)
                .WithMessage("The attachment content is null");

        }


    }
}
