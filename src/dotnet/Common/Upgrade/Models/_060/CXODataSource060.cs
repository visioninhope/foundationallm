using FoundationaLLM.Common.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._060
{
    public class CXODataSource060 : DataSourceBase060
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public CXOConfiguration040? Configuration { get; set; }
    }
}
