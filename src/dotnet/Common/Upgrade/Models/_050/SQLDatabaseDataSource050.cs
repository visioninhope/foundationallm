using FoundationaLLM.Common.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._050
{
    /// <summary>
    /// SQL Database data source metadata model.
    /// </summary>
    public class SQLDatabaseDataSource050 : DataSourceBase050
    {
        /// <summary>
        /// SQL Database configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SQLDatabaseConfiguration040? Configuration { get; set; }
    }
}
