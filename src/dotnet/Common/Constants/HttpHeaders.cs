using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants
{
    /// <summary>
    /// Common HTTP headers used throughout the FoundationaLLM infrastructure..
    /// </summary>
    public static class HttpHeaders
    {
        /// <summary>
        /// API key header used by APIs to authenticate requests.
        /// </summary>
        public const string APIKey = "X-API-KEY";
        /// <summary>
        /// User identity header used by APIs to pass user identity information.
        /// </summary>
        public const string UserIdentity = "X-USER-IDENTITY";

        /// <summary>
        /// The correlation id used to map all traces for a single user prompt
        /// </summary>
        public const string CorrelationId = "correlationId";
    }
}
