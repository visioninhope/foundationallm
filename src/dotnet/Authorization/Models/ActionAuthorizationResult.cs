using System.Text.Json.Serialization;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Represents the result of an action authorization request.
    /// </summary>
    public class ActionAuthorizationResult
    {
        /// <summary>
        /// Indicates whether the action is authorized or not.
        /// </summary>
        [JsonPropertyName("authorized")]
        public required bool Authorized { get; set; }
    }
}
