using System.Text.Json.Serialization;
using FoundationaLLM.Common.Constants.Orchestration;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// An OpenAI text message content item.
    /// </summary>
    public class OpenAITextMessageContentItem : MessageContentItemBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }

        /// <summary>
        /// A list of file annotations used to generate the message content item.
        /// </summary>
        [JsonPropertyName("annotations")]
        public List<OpenAIFilePathContentItem> Annotations { get; set; } = [];

        /// <summary>
        /// The text value of the message content item.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Set default property values.
        /// </summary>
        public OpenAITextMessageContentItem() =>
            Type = MessageContentItemTypes.Text;
    }
}
