namespace FoundationaLLM.Common.Models.ResourceProviders
{
    /// <summary>
    /// Maintains a collection of resource references.
    /// </summary>
    /// <typeparam name="T">The type of resource reference kept in the store.</typeparam>
    public class ResourceReferenceStore<T> where T : ResourceReference
    {
        /// <summary>
        /// The list of all resource references.
        /// </summary>
        public required List<T> ResourceReferences { get; set; }

        /// <summary>
        /// Creates a string-based dictionary of <typeparamref name="T"/> values from the current object.
        /// </summary>
        /// <returns>The string-based dictionary of <typeparamref name="T"/> values from the current object.</returns>
        public Dictionary<string, T> ToDictionary() =>
            ResourceReferences.ToDictionary<T, string>(ar => ar.Name);

        /// <summary>
        /// Creates a new instance of the <see cref="ResourceReferenceStore{T}"/> from a dictionary.
        /// </summary>
        /// <param name="dictionary">A string-based dictionary of <typeparamref name="T"/> values.</param>
        /// <returns>The <see cref="ResourceReferenceStore{T}"/> object created from the dictionary.</returns>
        public static ResourceReferenceStore<T> FromDictionary(Dictionary<string, T> dictionary) =>
            new()
            {
                ResourceReferences = [.. dictionary.Values]
            };
    }
}
