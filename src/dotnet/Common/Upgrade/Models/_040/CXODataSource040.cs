using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._040
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
