using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// Input for a direct Azure OpenAI request.
    /// </summary>
    public class AzureOpenAIDirectRequest
    {
        /// <summary>
        /// Input data for a direct request to an Azure AI model.
        /// </summary>
        [JsonPropertyName("input_data")]
        public AzureOpenAIDirectInputData? InputData { get; set; }
    }

    
}
