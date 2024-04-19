using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Vectorization
{
    /// <summary>
    /// Describes the state of a vectorization request.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VectorizationProcessingState
    {
        /// <summary>
        /// The vectorization request is new and has not been processed.
        /// </summary>
        New,

        /// <summary>
        /// The request is being processed.
        /// </summary>
        InProgress,

        /// <summary>
        /// All steps of the request have completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// An error occurred during processing.
        /// </summary>
        Failed
    }
}
