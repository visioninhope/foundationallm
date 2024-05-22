using FoundationaLLM.Utility.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._051
{
    /// <summary>
    /// Data Source base class.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "underlying_implementation")]
    [JsonDerivedType(typeof(BlobStorageDataSource040), "blob-storage")]
    [JsonDerivedType(typeof(CXODataSource040), "cxo")]
    [JsonDerivedType(typeof(SearchServiceDataSource040), "search-service")]
    [JsonDerivedType(typeof(SQLDatabaseDataSource040), "sql")]
    public class DataSourceBase051 : MetadataBase040
    {
        /// <summary>
        /// Descriptor for the type of data in the data source.
        /// </summary>
        /// <example>Survey data for a CSV file that contains survey results.</example>
        [JsonPropertyName("data_description")]
        public string? DataDescription { get; set; }
    }
}
