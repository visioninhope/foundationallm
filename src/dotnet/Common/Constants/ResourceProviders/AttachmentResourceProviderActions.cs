namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// The names of the actions implemented by the Attachment resource provider.
    /// </summary>
    public class AttachmentResourceProviderActions
    {
        /// <summary>
        /// Check the validity of a resource name.
        /// </summary>
        public const string CheckName = "checkname";
        /// <summary>
        /// Apply a filter for data source retrieval.
        /// </summary>
        public const string Filter = "filter";
        /// <summary>
        /// Purges a soft-deleted resource.
        /// </summary>
        public const string Purge = "purge";
        /// <summary>
        /// Loads a resource for creating embeddings
        /// </summary>
        public const string Load = "load";
    }
}
