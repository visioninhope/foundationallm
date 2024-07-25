using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Represents the current state of a long-running operation.
    /// </summary>
    public class LongRunningOperation
    {
        /// <summary>
        /// The identifier of the long-running operation.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id => OperationId;

        /// <summary>
        /// The document type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type => nameof(LongRunningOperation);

        /// <summary>
        /// The identifier of the long-running operation.
        /// </summary>
        [JsonPropertyName("operation_id")]
        public required string OperationId { get; set; }

        /// <summary>
        /// The status of the long-running operation.
        /// </summary>
        [JsonPropertyName("status")]
        public required OperationStatus Status { get; set; }

        /// <summary>
        /// The message describing the current state of the operation.
        /// </summary>
        [JsonPropertyName("status_message")]
        public string? StatusMessage { get; set; }

        /// <summary>
        /// The time stamp of the last update to the operation.
        /// </summary>
        [JsonPropertyName("last_updated")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Deleted flag used for soft delete.
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }
}
