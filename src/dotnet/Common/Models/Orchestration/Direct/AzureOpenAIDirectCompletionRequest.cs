using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// Input for a direct Azure OpenAI completion request.
    /// </summary>
    public class AzureOpenAIDirectCompletionRequest : AzureOpenAIDirectParameters
    {
        /// <summary>
        /// The prompt for which to generate completions.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }
    }
}
