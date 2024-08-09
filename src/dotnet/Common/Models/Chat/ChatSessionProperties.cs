using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Chat
{
    /// <summary>
    /// The session properties object.
    /// </summary>
    public class ChatSessionProperties
    {
        /// <summary>
        /// The session name.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
