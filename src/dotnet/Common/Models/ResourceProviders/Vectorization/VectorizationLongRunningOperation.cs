using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Vectorization
{
    /// <summary>
    /// Provides the details of a long-running vectorization operation.
    /// </summary>
    public class VectorizationLongRunningOperation
    {
        /// <summary>
        /// The unique identifier of the long-running operation.
        /// </summary>
        [JsonPropertyOrder(-5)]
        [JsonPropertyName("operation_id")]
        public required string OperationId { get; set; }

        /// <summary>
        /// The first time we learned about the operation being run.
        /// </summary>
        [JsonPropertyOrder(5)]
        [JsonPropertyName("first_response_time")]
        public DateTime FirstResponseTime { get; set; }

        /// <summary>
        /// The last time we learned about the operation being run.
        /// </summary>
        [JsonPropertyOrder(6)]
        [JsonPropertyName("last_response_time")]
        public DateTime LastResponseTime { get; set; }

        /// <summary>
        /// Indicates whether the operation is complete or not.
        /// </summary>
        [JsonPropertyOrder(10)]
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// The number of times an attempt was made to retrieve the result.
        /// </summary>
        [JsonPropertyOrder(15)]
        [JsonPropertyName("polling_count")]
        public int PollingCount { get; set; }
    }
}
