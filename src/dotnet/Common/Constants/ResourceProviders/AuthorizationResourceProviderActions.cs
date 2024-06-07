namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// The names of the actions implemented by the Authorization resource provider.
    /// </summary>
    public static class AuthorizationResourceProviderActions
    {
        /// <summary>
        /// Get user accounts.
        /// </summary>
        public const string GetUsers = "getUsers";
        /// <summary>
        /// Get security groups.
        /// </summary>
        public const string GetGroups = "getGroups";
        /// <summary>
        /// Get account objects (users and groups).
        /// </summary>
        public const string GetObjects = "getObjects";
        /// <summary>
        /// Filter role assignments.
        /// </summary>
        public const string Filter = "filter";
    }
}
