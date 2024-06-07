using FoundationaLLM.Common.Constants.Authentication;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Stores basic account information.
    /// </summary>
    public class AccountBase
    {
        /// <summary>
        /// The object type (user / group).
        /// </summary>
        [JsonIgnore]
        public virtual string? ObjectType => ObjectTypes.Other;

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
