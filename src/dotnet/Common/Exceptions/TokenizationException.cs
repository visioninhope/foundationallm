using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Exceptions
{
    /// <summary>
    /// Represents an error in the tokenization process.
    /// </summary>
    public class TokenizationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizationException"/> class with a default message.
        /// </summary>
        public TokenizationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizationException"/> class with its message set to <paramref name="message"/>.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public TokenizationException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizationException"/> class with its message set to <paramref name="message"/>.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public TokenizationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
