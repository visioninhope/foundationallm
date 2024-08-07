using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Chat
{
    /// <summary>
    /// Represents an attachment in a chat message or session.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// The unique identifier of the attachment resource.
        /// </summary>
        [JsonPropertyName("object_id")]
        public string? ObjectId { get; set; }

        /// <summary>
        /// The date the attachment was added.
        /// </summary>
        [JsonPropertyName("date_added")]
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// The attachment file name.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        /// <summary>
        /// The mime content type of the attachment.
        /// </summary>
        [JsonPropertyName("content_type")]
        public string? ContentType { get; set; }
    }
}
