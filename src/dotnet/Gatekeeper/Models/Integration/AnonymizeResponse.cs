using Newtonsoft.Json;

namespace FoundationaLLM.Gatekeeper.Core.Models.Integration
{
    public class AnonymizeResponse
    {
        [JsonProperty("content")]
        public required string Content { get; set; }

        [JsonProperty("results")]
        public required List<PIIResultAnonymized> Results { get; set; }
    }
}
