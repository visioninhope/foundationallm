using Newtonsoft.Json;

namespace FoundationaLLM.Gatekeeper.Core.Models.Integration
{
    public class AnalyzeRequest
    {
        [JsonProperty("content")]
        public required string Content { get; set; }

        [JsonProperty("anonymize")]
        public required bool Anonymize {  get; set; }

        [JsonProperty("language")]
        public string? Language {  get; set; }
    }
}
