namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Models the content of the role assignments store managed by the FoundationaLLM.Authorization resource provider.
    /// </summary>
    public class RoleAssignmentStore
    {
        /// <summary>
        /// The unique identifier of the FoundationaLLM instance.
        /// </summary>
        public required string InstanceId { get; set; }

        /// <summary>
        /// The list of all role assignments in the FoundationaLLM instance.
        /// </summary>
        public required List<RoleAssignment> RoleAssignments { get; set; } = [];
    }
}
