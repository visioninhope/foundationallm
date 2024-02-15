using FoundationaLLM.Common.Models.Orchestration;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Agents
{
    /// <summary>
    /// The Knowledge Management Completion Request model.
    /// </summary>
    public class InternalContextCompletionRequest : LLMCompletionRequest
    {
        /// <summary>
        /// The Internal Context agent metadata.
        /// </summary>
        [JsonPropertyName("agent")]
        public required InternalContextAgent Agent { get; set; }
    }
}
