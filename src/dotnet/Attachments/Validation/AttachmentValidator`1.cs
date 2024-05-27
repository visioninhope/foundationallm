using FluentValidation;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;

namespace FoundationaLLM.Attachment.Validation
{
    /// <summary>
    /// Base validator for attachments.
    /// </summary>
    /// <typeparam name="T">The type of attachment to validate.</typeparam>
    public class AttachmentValidator<T> : AbstractValidator<T> where T : AttachmentBase
    {
        /// <summary>
        /// Validates the value of a specified configuration reference.
        /// </summary>
        /// <param name="dataSource">The attachment object being validated.</param>
        /// <param name="configurationKey">The name of the configuration reference being validated.</param>
        /// <returns>True if the value of the configuration reference is valid, False otherwise.</returns>
        protected bool ValidConfigurationReference(AttachmentBase dataSource, string configurationKey) =>
            dataSource.ConfigurationReferences!.ContainsKey(configurationKey)
            && !string.IsNullOrWhiteSpace(dataSource.ConfigurationReferences[configurationKey])
            && (string.Compare(
                $"{AppConfigurationKeySections.FoundationaLLM_Attachments}:{dataSource.Name}:{configurationKey}",
                dataSource.ConfigurationReferences![configurationKey]) == 0);
    }
}
