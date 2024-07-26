using System.Text.Json.Serialization;
using FoundationaLLM.Common.Constants.Orchestration;

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
        public string? Id => OperationId;

        /// <summary>
        /// The document type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type => LongRunningOperationTypes.LongRunningOperation;

        /// <summary>
        /// The identifier of the long-running operation.
        /// </summary>
        [JsonPropertyName("operation_id")]
        public string? OperationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The status of the long-running operation.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
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
        public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The time-to-live (TTL) for the log entry.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int TTL { get; set; } = Convert.ToInt32(TimeSpan.FromSeconds(604800));
    }
}
