using FoundationaLLM.Common.Constants;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration.Direct
{
    /// <summary>
    /// User input message for a direct orchestration request.
    /// </summary>
    public class UserInputMessage : InputMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInputMessage"/> class.
        /// </summary>
        public UserInputMessage() =>
            Role = InputMessageRoles.User;

        /// <summary>
        /// An optional name for the participant. Provides the model information
        /// to differentiate between participants of the same role.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
    }
}
