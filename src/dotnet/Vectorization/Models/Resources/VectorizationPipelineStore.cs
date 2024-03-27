namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Store for the vectorization pipelines.
    /// </summary>
    public class VectorizationPipelineStore
    {
        /// <summary>
        /// The list of all <see cref="VectorizationPipeline"/> vectorization pipelines.
        /// </summary>
        public required List<VectorizationPipeline> Pipelines { get; set; }

        /// <summary>
        /// Creates a new pipeline store from a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary containing the pipelines.</param>
        /// <returns>The newly created pipeline store.</returns>
        public static VectorizationPipelineStore FromDictionary(Dictionary<string, VectorizationPipeline> dictionary) =>
            new VectorizationPipelineStore
            {
                Pipelines = [.. dictionary.Values]
            };

        /// <summary>
        /// Creates a dictionary of pipelines from the pipeline store.
        /// </summary>
        /// <returns>The newly created dictionary.</returns>
        public Dictionary<string, VectorizationPipeline> ToDictionary() =>
            Pipelines.ToDictionary<VectorizationPipeline, string>(p => p.Name);
    }
}
