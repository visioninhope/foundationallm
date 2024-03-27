using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.ResourceProvider
{
    /// <summary>
    /// Filters resources based on the specified criteria.
    /// </summary>
    public class ResourceFilter
    {
        /// <summary>
        /// Specify whether to filter by resources designated as default.
        /// If null, the filter will not be applied. If true, only default resources will be returned.
        /// </summary>
        [JsonPropertyName("default")]
        public bool? Default { get; set; }
    }
}
