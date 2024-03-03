using FoundationaLLM.Common.Models.ResourceProvider;
using System.Text.Json.Serialization;

namespace FoundationaLLM.DataSource.Models
{
    /// <summary>
    /// Basic data source.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    public class DataSourceBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
