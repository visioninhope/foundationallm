namespace FoundationaLLM.Vectorization.Constants
{
    /// <summary>
    /// The names of the actions implemented by the Agent resource provider.
    /// </summary>
    public static class VectorizationResourceProviderActions
    {
        /// <summary>
        /// Check the validity of a resource name.
        /// </summary>
        public const string CheckName = "checkname";

        /// <summary>
        /// Apply a filter for vectorization resource retrieval.
        /// </summary>
        public const string Filter = "filter";

        /// <summary>
        /// Activate a vectorization pipeline.
        /// </summary>
        public const string Activate = "activate";

        /// <summary>
        /// Deactivate a vectorization pipeline.
        /// </summary>
        public const string Deactivate = "deactivate";
    }
}
