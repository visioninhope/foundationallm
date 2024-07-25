using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// The point-in-time state of a long-running operation.
    /// </summary>
    public class LongRunningOperationLogEntry
    {
        /// <summary>
        /// The identifier of the long-running operation log entry.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The document type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type => nameof(LongRunningOperationLogEntry);

        /// <summary>
        /// The identifier of the long-running operation.
        /// </summary>
        [JsonPropertyName("operation_id")]
        public string OperationId { get; set; }

        /// <summary>
        /// The status of the long-running operation.
        /// </summary>
        [JsonPropertyName("status")]
        public OperationStatus Status { get; set; }

        /// <summary>
        /// The time stamp of the log entry.
        /// </summary>
        [JsonPropertyName("time_stamp")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The message describing the current state of the operation.
        /// </summary>
        [JsonPropertyName("status_message")]
        public string? StatusMessage { get; set; }

        /// <summary>
        /// The time-to-live (TTL) for the log entry.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int TTL { get; set; } = TimeSpan.FromDays(7).Seconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="LongRunningOperationLogEntry"/> class.
        /// </summary>
        public LongRunningOperationLogEntry()
        {
            Id = Guid.NewGuid().ToString();
            TimeStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongRunningOperationLogEntry"/> class with the specified values.
        /// </summary>
        /// <param name="operationId">The identifier of the long-running operation.</param>
        /// <param name="status">The status of the long-running operation.</param>
        /// <param name="statusMessage">The message describing the current state of the operation.</param>
        public LongRunningOperationLogEntry(string operationId, OperationStatus status, string statusMessage)
        {
            Id = Guid.NewGuid().ToString();
            OperationId = operationId;
            Status = status;
            StatusMessage = statusMessage;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
