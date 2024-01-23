using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Management.Models.Configuration.Cache
{
    /// <summary>
    /// Contains the result of a cache refresh operation.
    /// </summary>
    public class APICacheRefreshResult
    {
        /// <summary>
        /// Details of the cache refresh operation from the called API.
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// Indicates whether the cache refresh operation was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
