using FoundationaLLM.Common.Models.ResourceProvider;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Represents a security role assignment.
    /// </summary>
    public class RoleAssignment : ResourceBase
    {
        /// <summary>
        /// The unique identifier of the role definition.
        /// </summary>
        [JsonPropertyName("role_definition_id")]
        public required string RoleDefinitionId { get; set; }

        /// <summary>
        /// The unique identifier of the security principal to which the role is assigned.
        /// </summary>
        [JsonPropertyName("principal_id")]
        public required string PrincipalId { get; set; }

        /// <summary>
        /// The type of the security principal to which the role is assigned. Can be User, Group, or ServicePrincipal.
        /// </summary>
        [JsonPropertyName("principal_type")]
        public required string PrincipalType { get; set; }

        /// <summary>
        /// The scope at which the role is assigned.
        /// </summary>
        [JsonPropertyName("scope")]
        public required string Scope { get; set; }
    }
}
