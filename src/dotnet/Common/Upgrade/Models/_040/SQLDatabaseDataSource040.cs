using FoundationaLLM.Common.Models.Orchestration.DataSourceConfigurations;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._040
{
    /// <summary>
    /// SQL Database data source metadata model.
    /// </summary>
    public class SQLDatabaseDataSource040 : DataSourceBase040
    {
        /// <summary>
        /// SQL Database configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SQLDatabaseConfiguration040? Configuration { get; set; }
    }
}
