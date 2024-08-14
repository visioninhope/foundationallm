namespace FoundationaLLM.Common.Models.ResourceProviders
{
    /// <summary>
    /// Maintains a collection of resource references.
    /// </summary>
    /// <typeparam name="T">The type of resource reference kept in the store.</typeparam>
    public class ResourceReferenceList<T> where T : ResourceReference
    {
        /// <summary>
        /// The dictionary of resource references indexed by their unique names.
        /// </summary>
        public required Dictionary<string, T> ResourceReferences { get; set; }
    }
}
