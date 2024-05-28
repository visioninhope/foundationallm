using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.ResourceProviders.Attachment
{
    /// <summary>
    /// Concrete type for passing in or returning audio file content stream with Core API
    /// </summary>
    public class AudioAttachmentStreamed : AudioAttachment
    {
        /// <summary>
        /// Stream of uploaded audio file from the Core API
        /// </summary>
        public Stream? Content { get; set; }
    }
}
