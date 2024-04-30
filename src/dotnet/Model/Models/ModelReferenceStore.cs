namespace FoundationaLLM.Model.Models
{
    /// <summary>
    /// Models the content of the model reference store managed by the FoundationaLLM.Model resource provider.
    /// </summary>
    public class ModelReferenceStore
    {
        /// <summary>
        /// The list of all models registered in the system.
        /// </summary>
        public required List<ModelReference> ModelReferences { get; set; }

        /// <summary>
        /// Creates a string-based dictionary of <see cref="ModelReference"/> values from the current object.
        /// </summary>
        /// <returns>The string-based dictionary of <see cref="ModelReference"/> values form the current object.</returns>
        public Dictionary<string, ModelReference> ToDictionary() =>
            ModelReferences.ToDictionary(mr => mr.Name);

        /// <summary>
        /// Creates a new instance of the <see cref="ModelReferenceStore"/> from a dictionary.
        /// </summary>
        /// <param name="dictionary">A string-based dictionary of <see cref="ModelReference"/> values.</param>
        /// <returns>The <see cref="ModelReferenceStore"/> object created from the dictionary.</returns>
        public static ModelReferenceStore FromDictionary(Dictionary<string, ModelReference> dictionary) =>
            new()
            {
                ModelReferences = [.. dictionary.Values]
            };
    }
}
