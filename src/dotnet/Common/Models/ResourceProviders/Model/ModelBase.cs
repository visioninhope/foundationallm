using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.Model
{
    /// <summary>
    /// Basic model.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    public class ModelBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
