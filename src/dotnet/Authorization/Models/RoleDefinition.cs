using FoundationaLLM.Common.Models.ResourceProvider;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Contains information about a security role definition.
    /// </summary>
    public class RoleDefinition : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override bool Deleted { get; set; }

        /// <summary>
        /// The list of scopes at which the role can be assigned.
        /// </summary>
        [JsonPropertyName("assignable_scopes")]
        [JsonPropertyOrder(1)]
        public List<string> AssignableScopes { get; set; } = [];

        /// <summary>
        /// The permissions associated with the security role definition.
        /// </summary>
        [JsonPropertyName("permissions")]
        [JsonPropertyOrder(2)]
        public List<RoleDefinitionPermissions> Permissions { get; set; } = [];

        /// <summary>
        /// The time at which the security role definition was created.
        /// </summary>
        [JsonPropertyName("created_on")]
        [JsonPropertyOrder(3)]
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// The time at which the security role definition was last updated.
        /// </summary>
        [JsonPropertyName("updated_on")]
        [JsonPropertyOrder(4)]
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// The entity who created the security role definition.
        /// </summary>
        [JsonPropertyName("created_by")]
        [JsonPropertyOrder(5)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// The entity who last updated the security role definition.
        /// </summary>
        [JsonPropertyName("updated_by")]
        [JsonPropertyOrder(6)]
        public string? UpdatedBy { get; set; }
    }
}
