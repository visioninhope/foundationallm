using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    /// <summary>
    /// Search service data source metadata model.
    /// </summary>
    public class SearchServiceDataSource040 : DataSourceBase040
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SearchServiceConfiguration040? Configuration { get; set; }

    }
}
