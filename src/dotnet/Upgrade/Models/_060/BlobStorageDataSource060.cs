using FoundationaLLM.Upgrade.Models._050;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Blob storage data source metadata model.
    /// </summary>
    public class BlobStorageDataSource060 : DataSourceBase060
    {
        /// <summary>
        /// Blob storage configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public BlobStorageConfiguration050? Configuration { get; set; }
    }
}
