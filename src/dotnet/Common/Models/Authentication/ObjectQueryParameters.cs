using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Parameters for querying objects.
    /// </summary>
    public class ObjectQueryParameters
    {
        /// <summary>
        /// The IDs of the objects to query.
        /// </summary>
        [JsonPropertyName("ids")]
        public required string[] Ids { get; set; }
    }
}
