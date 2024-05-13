using FoundationaLLM.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// SQL Database data source metadata model.
    /// </summary>
    public class SQLDatabaseDataSource060 : DataSourceBase060
    {
        /// <summary>
        /// SQL Database configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SQLDatabaseConfiguration040? Configuration { get; set; }
    }
}
