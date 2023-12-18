using Newtonsoft.Json;

namespace FoundationaLLM.Gatekeeper.Core.Models.Integration
{
    public class PIIResultAnonymized : PIIResult
    {
        [JsonProperty("anonymized_text")]
        public required string AnonymizedText { get; set; }

        [JsonProperty("operator")]
        public required string Operator { get; set; }
    }
}
