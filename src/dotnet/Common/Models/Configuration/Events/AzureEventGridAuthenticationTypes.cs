using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Configuration.Events
{
    /// <summary>
    /// Types of authentication for Azure Event Grid namespaces.
    /// </summary>
    public enum AzureEventGridAuthenticationTypes
    {
        /// <summary>
        /// Unknown authentication type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Azure managed identity authentication type.
        /// </summary>
        AzureIdentity,

        /// <summary>
        /// API key authentication type.
        /// </summary>
        APIKey
    }
}
