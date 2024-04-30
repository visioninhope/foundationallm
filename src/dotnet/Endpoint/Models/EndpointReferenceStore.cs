namespace FoundationaLLM.Endpoint.Models
{
    /// <summary>
    /// Models the content of the endpoint reference store managed by the FoundationaLLM.Endpoint resource provider.
    /// </summary>
    public class EndpointReferenceStore
    {
        /// <summary>
        /// The list of all endpoints registered in the system.
        /// </summary>
        public required List<EndpointReference> EndpointReferences { get; set; }

        /// <summary>
        /// Creates a string-based dictionary of <see cref="EndpointReference"/> values from the current object.
        /// </summary>
        /// <returns>The string-based dictionary of <see cref="EndpointReference"/> values from the current object.</returns>
        public Dictionary<string, EndpointReference> ToDictionary() =>
            EndpointReferences.ToDictionary<EndpointReference, string>(er => er.Name);

        /// <summary>
        /// Creates a new instance of the <see cref="EndpointReferenceStore"/> from a dictionary.
        /// </summary>
        /// <param name="dictionary">A string-based dictionary of <see cref="EndpointReference"/> values.</param>
        /// <returns>The <see cref="EndpointReference"/> object created from the dictionary.</returns>
        public static EndpointReferenceStore FromDictionary(Dictionary<string, EndpointReference> dictionary) =>
            new()
            {
                EndpointReferences = [.. dictionary.Values]
            };
    }
}
