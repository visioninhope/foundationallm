using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models
{
    public class VectorizationLogEntry
    {
        [JsonPropertyOrder(0)]
        [JsonPropertyName("t")]
        public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;

        [JsonPropertyOrder(1)]
        [JsonPropertyName("src")]
        public string Source { get; set; }

        [JsonPropertyOrder(2)]
        [JsonPropertyName("txt")]
        public string Text { get; set; }

        public VectorizationLogEntry(string source, string text)
        {
            this.Source = source;
            this.Text = text;
        }
    }
}
