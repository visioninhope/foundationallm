using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Authorization
{
    /// <summary>
    /// Represents the result of a batch action authorization request.
    /// </summary>
    public class BatchActionAuthorizationResult
    {
        /// <summary>
        /// Indicates whether the action is authorized or not for each resource path.
        /// </summary>
        [JsonPropertyName("authorized")]
        public required Dictionary<string, bool> Authorized { get; set; }
    }
}
