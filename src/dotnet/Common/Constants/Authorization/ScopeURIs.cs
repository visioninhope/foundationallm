using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants.Authorization
{
    /// <summary>
    /// The URIs for the scopes used in the application.
    /// </summary>
    public static class ScopeURIs
    {
        /// <summary>
        /// The Application ID URI for the Core API.
        /// </summary>
        public const string FoundationaLLM_Core = "api://FoundationaLLM-Auth"; //"api://FoundationaLLM-Core";
        /// <summary>
        /// The Application ID URI for the Authorization API.
        /// </summary>
        public const string FoundationaLLM_Authorization = "api://FoundationaLLM-Authorization-Auth"; //"api://FoundationaLLM-Authorization";
        /// <summary>
        /// The Application ID URI for the Management API.
        /// </summary>
        public const string FoundationaLLM_Management = "api://FoundationaLLM-Management-Auth"; //"api://FoundationaLLM-Management";
    }
}
