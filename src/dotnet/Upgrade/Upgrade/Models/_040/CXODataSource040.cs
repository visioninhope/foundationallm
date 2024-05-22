using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    public class CXODataSource040 : DataSourceBase040
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public CXOConfiguration040? Configuration { get; set; }
    }
}
