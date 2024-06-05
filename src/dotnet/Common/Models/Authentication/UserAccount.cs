using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Stores user account information.
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// User account identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// User account name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// User account email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}
