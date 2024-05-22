using FoundationaLLM.Utility.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Search service data source metadata model.
    /// </summary>
    public class SearchServiceDataSource050 : DataSourceBase050
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SearchServiceConfiguration040? Configuration { get; set; }

    }
}
