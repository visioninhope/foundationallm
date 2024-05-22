using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    /// <summary>
    /// Contains information about a security role definition.
    /// </summary>
    public class ResourceDefinition040 : ResourceBase040
    {
        /// <inheritdoc/>
        //[JsonIgnore]
        //public override bool Deleted { get; set; }

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
        public List<RoleDefinitionPermissions040> Permissions { get; set; } = [];
    }
}
