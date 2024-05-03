using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// Actions that can be performed by the base resource provider and any of its children.
    /// </summary>
    public static class BaseResourceProviderActions
    {
        /// <summary>
        /// Purges a soft-deleted resource.
        /// </summary>
        public const string Purge = "purge";
    }
}
