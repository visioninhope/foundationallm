using System.Text.Json.Serialization;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Represents a request to authorize an action.
    /// </summary>
    public class ActionAuthorizationRequest
    {
        /// <summary>
        /// The FoundationaLLM instance id to which the request is targeted.
        /// </summary>
        [JsonPropertyName("instance_id")]
        public required string InstanceId { get; set; }

        /// <summary>
        /// The authorizable action for which authorization is requested.
        /// </summary>
        [JsonPropertyName("action")]
        public required string Action { get; set; }

        /// <summary>
        /// The resource for which authorization is requested.
        /// </summary>
        [JsonPropertyName("resource")]
        public required string Resource { get; set; }

        /// <summary>
        /// The id of the security principal requesting authorization.
        /// </summary>
        [JsonPropertyName("principal_id")]
        public required string PrincipalId { get; set; }

        /// <summary>
        /// The list of security group ids to which the principal belongs.
        /// </summary>
        [JsonPropertyName("security_group_ids")]
        public List<string> SecurityGroupIds { get; set; } = [];    
    }
}
