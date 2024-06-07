using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Parameters for querying user and group accounts.
    /// </summary>
    public class AccountQueryParameters
    {
        /// <summary>
        /// Account name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Account id.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        [JsonPropertyName("page_number")]
        public int? PageNumber { get; set; }

        /// <summary>
        /// The number of items to return in each page.
        /// </summary>
        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
    }
}
