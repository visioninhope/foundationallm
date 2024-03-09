using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// The response from the Azure OpenAI orchestration service.
    /// </summary>
    public class AzureOpenAIDirectResponse
    {
        /// <summary>
        /// The completion response ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// The completion response object.
        /// </summary>
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Object { get; set; }
        /// <summary>
        /// The completion response created timestamp.
        /// </summary>
        [JsonPropertyName("created")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Created { get; set; }
        /// <summary>
        /// The completion model used for the response.
        /// </summary>
        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Model { get; set; }
        /// <summary>
        /// The completion response choices.
        /// </summary>
        [JsonPropertyName("choices")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Choice[]? Choices { get; set; }
    }

    /// <summary>
    /// The completion response choice.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// The completion response text.
        /// </summary>
        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }
        /// <summary>
        /// The completion response index.
        /// </summary>
        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Index { get; set; }
        /// <summary>
        /// The log probabilities on the logprobs most likely tokens, as well the chosen tokens.
        /// </summary>
        [JsonPropertyName("logprobs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogProbs { get; set; }
        /// <summary>
        /// The finish reason for the completion response.
        /// </summary>
        [JsonPropertyName("finish_reason")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FinishReason { get; set; }
    }

}
