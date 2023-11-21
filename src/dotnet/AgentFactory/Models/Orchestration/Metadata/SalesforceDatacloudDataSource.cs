using FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations;
using Newtonsoft.Json;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.Metadata
{
    /// <summary>
    /// Blob storage data source metadata model.
    /// </summary>
    public class SalesforceDatacloudDataSource : DataSourceBase
    {
        /// <summary>
        /// Blob storage configuration settings.
        /// </summary>
        [JsonProperty("configuration")]
        public SalesforceDatacloudConfiguration? Configuration { get; set; }
    }
}