using FoundationaLLM.Common.Models.Metadata;
using System.Text.Json.Serialization;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSources
{
    /// <summary>
    /// Data Source base class.
    /// </summary>
    public class DataSourceBase : MetadataBase
    {
        /// <summary>
        /// Descriptor for the type of data in the data source.
        /// </summary>
        /// <example>Survey data for a CSV file that contains survey results.</example>
        [JsonPropertyName("data_description")]
        public string? DataDescription { get; set; }
    }
}
