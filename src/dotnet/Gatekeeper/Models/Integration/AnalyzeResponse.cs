using Newtonsoft.Json;

namespace FoundationaLLM.Gatekeeper.Core.Models.Integration
{
    public class AnalyzeResponse
    {
        [JsonProperty("content")]
        public required string Content { get; set; }

        [JsonProperty("results")]
        public required List<PIIResult> Results { get; set; }
    }
}
