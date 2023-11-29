using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models.Configuration
{
    /// <summary>
    /// Provides configuration settings to initialize a request manager instance.
    /// </summary>
    public record RequestManagerServiceSettings
    {
        /// <summary>
        /// The name of the request source that provides the requests processed by the request manager.
        /// </summary>
        public required string RequestSourceName;

        /// <summary>
        /// The maximum number of handler instances allowed to run in parallel.
        /// </summary>
        public int MaxHandlerInstances;
    }
}
