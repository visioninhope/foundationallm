using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// File content item used to generate a message content item.
    /// </summary>
    public class OpenAIFilePathContentItem
    {
        /// <summary>
        /// The text of the annotation.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>
        /// The starting index of the annotation.
        /// </summary>
        [JsonPropertyName("start_index")]
        public int? StartIndex { get; set; }

        /// <summary>
        /// The ending index of the annotation.
        /// </summary>
        [JsonPropertyName("end_index")]
        public int? EndIndex { get; set; }

        /// <summary>
        /// The ID of the file referenced by the annotation.
        /// </summary>
        [JsonPropertyName("file_id")]
        public string? FileId { get; set; }
    }
}
