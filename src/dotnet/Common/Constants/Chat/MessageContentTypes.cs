using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants.Chat
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageContentTypes
    {
        /// <summary>
        /// Plaintext and formatted text, such as markdown.
        /// </summary>
        public const string Text = "text";

        /// <summary>
        /// Image file link.
        /// </summary>
        public const string Image = "image";

        /// <summary>
        /// General file link.
        /// </summary>
        public const string File = "file";

        /// <summary>
        /// HTML file link.
        /// </summary>
        public const string Html = "html";
    }
}
