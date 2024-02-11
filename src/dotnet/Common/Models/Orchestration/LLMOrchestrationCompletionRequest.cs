using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Base LLM orchestration request
    /// </summary>
    public class LLMOrchestrationCompletionRequest
    {
        /// <summary>
        /// The session ID.
        /// </summary>
        [JsonPropertyName("session_id")]
        public string? SessionId { get; set; }

        /// <summary>
        /// Prompt entered by the user.
        /// </summary>
        [JsonPropertyName("user_prompt")]
        public string? UserPrompt { get; set; }
    }
}
