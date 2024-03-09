using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// Input for a direct Azure AI request.
    /// </summary>
    public class AzureAIDirectRequest
    {
        /// <summary>
        /// Input data for a direct request to an Azure AI model.
        /// </summary>
        [JsonPropertyName("input_data")]
        public AzureAIDirectInputData? InputData { get; set; }
    }
}
