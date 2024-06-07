using FoundationaLLM.Common.Constants.Authentication;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Stores user account information.
    /// </summary>
    public class UserAccount : AccountBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("object_type")]
        public override string ObjectType => ObjectTypes.User;

        /// <summary>
        /// User account email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}
