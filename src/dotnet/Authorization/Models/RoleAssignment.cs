using System.Text.Json.Serialization;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Represents a security role assignment.
    /// </summary>
    public class RoleAssignment
    {
        /// <summary>
        /// The unique identifier of the role assignment.
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; set; }
    }
}
