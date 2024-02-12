using FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations;
using System.Text.Json.Serialization;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSources
{
    /// <summary>
    /// CSV file data source metadata model.
    /// </summary>
    public class CSVFileDataSource : DataSourceBase
    {
        /// <summary>
        /// CSV file configuration settings metadata.
        /// </summary>
        [JsonPropertyName("configuration")]
        public CSVFileConfiguration? Configuration { get; set; }
    }
}
