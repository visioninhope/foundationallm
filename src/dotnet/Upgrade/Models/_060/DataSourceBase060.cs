using FoundationaLLM.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._060
{
    /// <summary>
    /// Data Source base class.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "underlying_implementation")]
    [JsonDerivedType(typeof(BlobStorageDataSource060), "blob-storage")]
    [JsonDerivedType(typeof(CXODataSource060), "cxo")]
    [JsonDerivedType(typeof(SearchServiceDataSource060), "search-service")]
    [JsonDerivedType(typeof(SQLDatabaseDataSource060), "sql")]
    public class DataSourceBase060 : MetadataBase040
    {
        /// <summary>
        /// Descriptor for the type of data in the data source.
        /// </summary>
        /// <example>Survey data for a CSV file that contains survey results.</example>
        [JsonPropertyName("data_description")]
        public string? DataDescription { get; set; }
    }
}
