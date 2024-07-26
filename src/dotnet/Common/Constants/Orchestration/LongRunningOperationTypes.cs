using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants.Orchestration
{
    /// <summary>
    /// The types of long-running operations.
    /// </summary>
    public static class LongRunningOperationTypes
    {
        /// <summary>
        /// The document type for a long-running operation.
        /// </summary>
        public const string LongRunningOperation = "LongRunningOperation";

        /// <summary>
        /// The document type for a long-running operation log entry.
        /// </summary>
        public const string LongRunningOperationLogEntry = "LongRunningOperationLogEntry";

        /// <summary>
        /// The document type for a long-running operation result.
        /// </summary>
        public const string LongRunningOperationResult = "LongRunningOperationResult";
    }
}
