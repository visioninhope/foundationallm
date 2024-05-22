using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// Blob storage data source metadata model.
    /// </summary>
    public class BlobStorageDataSource050 : DataSourceBase050
    {
        /// <summary>
        /// Blob storage configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public BlobStorageConfiguration050? Configuration { get; set; }
    }
}
