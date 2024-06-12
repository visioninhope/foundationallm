using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Parameters for querying objects.
    /// </summary>
    public class ObjectQueryParameters
    {
        /// <summary>
        /// The IDs of the objects to query.
        /// </summary>
        [JsonPropertyName("ids")]
        public required string[] Ids { get; set; }
    }
}
