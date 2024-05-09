using FoundationaLLM.Common.Models.ResourceProviders.DataSource;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class DataSourceCatalog
    {
        public static readonly List<DataSourceBase> Items =
        [
            new AzureDataLakeDataSource { Name = "datalake_vectorization_input", DisplayName = "datalake_vectorization_input", ConfigurationReferences = new Dictionary<string, string> { { "AuthenticationType", "FoundationaLLM:DataSources:datalake_vectorization_input:AuthenticationType" }, { "AccountName", "FoundationaLLM:DataSources:datalake_vectorization_input:AccountName" } }, Folders = new List<string> { "vectorization-input" } }
            
        ];

        public static List<DataSourceBase> GetDataSources()
        {
            var items = new List<DataSourceBase>();
            items.AddRange(Items);
            return items;
        }
    }
}
