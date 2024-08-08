using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Chat
{
    /// <summary>
    /// The session properties object.
    /// </summary>
    public class SessionProperties
    {
        /// <summary>
        /// The session name.
        /// </summary>
        [JsonPropertyName("session_name")]
        public required string SessionName { get; set; }
    }
}
