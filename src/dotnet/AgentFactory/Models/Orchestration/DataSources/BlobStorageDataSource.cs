using FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations;
using System.Text.Json.Serialization;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSources
{
    /// <summary>
    /// Blob storage data source metadata model.
    /// </summary>
    public class BlobStorageDataSource : DataSourceBase
    {
        /// <summary>
        /// Blob storage configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public BlobStorageConfiguration? Configuration { get; set; }
    }
}
