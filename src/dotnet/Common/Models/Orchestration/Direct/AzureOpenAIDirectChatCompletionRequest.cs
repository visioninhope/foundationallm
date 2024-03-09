using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// Input for a direct Azure OpenAI chat completion request.
    /// </summary>
    public class AzureOpenAIDirectChatCompletionRequest : AzureOpenAIDirectParameters
    {
        /// <summary>
        /// Object defining the required input role and content key value pairs.
        /// </summary>
        [JsonPropertyName("messages")]
        public InputMessage[]? Messages { get; set; }
    }
}
