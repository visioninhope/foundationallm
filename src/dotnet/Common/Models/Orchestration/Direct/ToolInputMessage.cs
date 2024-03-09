using FoundationaLLM.Common.Constants;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// Tool input message for a direct orchestration request.
    /// </summary>
    public class ToolInputMessage : InputMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolInputMessage"/> class.
        /// </summary>
        public ToolInputMessage() =>
            Role = InputMessageRoles.Tool;

        /// <summary>
        /// Tool call to which this message is responding.
        /// </summary>
        [JsonPropertyName("tool_call_id")]
        public required string ToolCallId { get; set; }
    }
}
