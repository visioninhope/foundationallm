using FoundationaLLM.Common.Constants;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// System input message for a direct orchestration request.
    /// </summary>
    public class SystemInputMessage : InputMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInputMessage"/> class.
        /// </summary>
        public SystemInputMessage() =>
            Role = InputMessageRoles.System;

        /// <summary>
        /// An optional name for the participant. Provides the model information
        /// to differentiate between participants of the same role.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
    }
}
