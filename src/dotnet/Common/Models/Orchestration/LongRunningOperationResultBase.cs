using FoundationaLLM.Common.Constants.Orchestration;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Base model for a long-running operation result.
    /// </summary>
    public class LongRunningOperationResultBase
    {
        /// <summary>
        /// The identifier of the long-running operation log entry.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The Operation ID identifying the completion request.
        /// </summary>
        [JsonPropertyName("operation_id")]
        public string OperationId { get; set; }

        /// <summary>
        /// The type of the document.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = LongRunningOperationTypes.LongRunningOperationResult;
    }
}
