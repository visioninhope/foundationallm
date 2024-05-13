using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._040
{
    /// <summary>
    /// Blob storage data source metadata model.
    /// </summary>
    public class BlobStorageDataSource040 : DataSourceBase040
    {
        /// <summary>
        /// Blob storage configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public BlobStorageConfiguration040? Configuration { get; set; }
    }
}
