using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Chat
{
    public class SessionAttachment
    {
        /// <summary>
        /// The unique identifier of the attachment resource.
        /// </summary>
        [JsonPropertyName("object_id")]
        public string? ObjectId { get; set; }

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
