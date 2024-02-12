using FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations;
using System.Text.Json.Serialization;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSources
{
    /// <summary>
    /// Search service data source metadata model.
    /// </summary>
    public class SearchServiceDataSource : DataSourceBase
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SearchServiceConfiguration? Configuration { get; set; }

    }
}
