using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Stores security group account information.
    /// </summary>
    public class GroupAccount
    {
        /// <summary>
        /// User account identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Group name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
