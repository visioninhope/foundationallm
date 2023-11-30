using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Configuration
{
    public class VectorizationWorkerSettings
    {
        [JsonPropertyOrder(0)]
        [JsonPropertyName("request_managers")]
        public List<RequestManagerServiceSettings>? RequestManagers { get; set; }

        [JsonPropertyOrder(1)]
        [JsonPropertyName("queuing_engine")]
        public VectorizationQueuing QueuingEngine { get; set; }
    }
}
