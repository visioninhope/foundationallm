namespace FoundationaLLM.Authorization.Constants
{
    /// <summary>
    /// Provides the names of the authorizable actions managed by the FoundationaLLM.Authorization provider.
    /// </summary>
    public static class AuthorizableActionNames
    {
        #region Authorization

        /// <summary>
        /// Read role assignments.
        /// </summary>
        public const string FoundationaLLM_Authorization_RoleAssignments_Read = "FoundationaLLM.Authorization/roleAssignments/read";

        /// <summary>
        /// Create or update role assignments.
        /// </summary>
        public const string FoundationaLLM_Authorization_RoleAssignments_Write = "FoundationaLLM.Authorization/roleAssignments/write";

        /// <summary>
        /// Delete role assignments.
        /// </summary>
        public const string FoundationaLLM_Authorization_RoleAssignments_Delete = "FoundationaLLM.Authorization/roleAssignments/delete";

        #endregion
    }
}