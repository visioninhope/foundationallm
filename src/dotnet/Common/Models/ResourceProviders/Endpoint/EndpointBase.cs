using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// Basic endpoint.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    public class EndpointBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
