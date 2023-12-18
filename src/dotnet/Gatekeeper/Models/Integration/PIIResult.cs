using Newtonsoft.Json;

namespace FoundationaLLM.Gatekeeper.Core.Models.Integration
{
    public class PIIResult
    {
        [JsonProperty("entity_type")]
        public required string EntityType {  get; set; }

        [JsonProperty("start_index")]
        public required int StartIndex { get; set; }

        [JsonProperty("end_index")]
        public required int EndIndex { get; set; }
    }
}
