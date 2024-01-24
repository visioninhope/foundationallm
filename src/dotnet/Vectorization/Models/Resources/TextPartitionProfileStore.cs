namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Models the content of the text partition profiles store managed by the FoundationaLLM.Vectorization resource provider.
    /// </summary>
    public class TextPartitionProfileStore
    {
        /// <summary>
        /// The list of all partition profiles that are registered for use by the vectorization pipelines.
        /// </summary>
        public required List<TextPartitionProfile> TextPartitioningProfiles { get; set; }
    }
}
