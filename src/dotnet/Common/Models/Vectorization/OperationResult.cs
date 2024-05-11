using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Base for a gateway operation result.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Indicates whether the operation is still in progress.
        /// When true, the <see cref="OperationId"/> property contains an operation identifier.
        /// </summary>
        [JsonPropertyName("in_progress")]
        public bool InProgress { get; set; }

        /// <summary>
        /// Indicates whether the operation failed due to an error.
        /// When true, the <see cref="ErrorMessage"/> property contains a message describing the error.
        /// </summary>
        [JsonPropertyName("cancelled")]
        public bool Failed { get; set; }

        /// <summary>
        /// The message describing the error that lead to the cancellation of the operation.
        /// </summary>
        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Optional operation identifier that can be used to retrieve the final result.
        /// </summary>
        [JsonPropertyName("operation_id")]
        public string? OperationId { get; set; }

        /// <summary>
        /// The number of tokens used during the operation.
        /// </summary>
        [JsonPropertyName("token_count")]
        public int TokenCount { get; set; }
    }
}
