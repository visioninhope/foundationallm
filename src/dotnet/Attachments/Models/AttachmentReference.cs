using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Attachment.Models
{
    /// <summary>
    /// Contains a reference to an attachment ..
    /// </summary>
    public class AttachmentReference : ResourceReference
    {
        /// <summary>
        /// The object type of the attachment.
        /// </summary>
        [JsonIgnore]
        public Type AttachmentType =>
            Type switch
            {
                AttachmentTypes.Audio => typeof(AudioBase),
                _ => throw new ResourceProviderException($"The attachment type {Type} is not supported.")
            };
    }
}
