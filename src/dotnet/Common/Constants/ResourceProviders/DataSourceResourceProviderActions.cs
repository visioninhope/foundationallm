namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// The names of the actions implemented by the Data Source resource provider.
    /// </summary>
    public class DataSourceResourceProviderActions
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
    }
}
