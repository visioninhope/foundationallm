using FoundationaLLM.Common.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._050
{
    /// <summary>
    /// Data Source base class.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "underlying_implementation")]
    [JsonDerivedType(typeof(BlobStorageDataSource050), "blob-storage")]
    [JsonDerivedType(typeof(CXODataSource050), "cxo")]
    [JsonDerivedType(typeof(SearchServiceDataSource050), "search-service")]
    [JsonDerivedType(typeof(SQLDatabaseDataSource050), "sql")]
    public class DataSourceBase050 : MetadataBase040
    {
        /// <summary>
        /// Descriptor for the type of data in the data source.
        /// </summary>
        /// <example>Survey data for a CSV file that contains survey results.</example>
        [JsonPropertyName("data_description")]
        public string? DataDescription { get; set; }
    }
}
