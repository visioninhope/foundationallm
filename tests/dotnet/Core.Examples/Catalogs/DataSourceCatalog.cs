using FoundationaLLM.Common.Models.ResourceProviders.DataSource;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class DataSourceCatalog
    {
        public static readonly List<DataSourceBase> Items =
        [
            new AzureDataLakeDataSource { Name = "really_big", DisplayName = "really_big", ConfigurationReferences = new Dictionary<string, string> { { "AuthenticationType", "FoundationaLLM:DataSources:really_big:AuthenticationType" }, { "AccountName", "FoundationaLLM:DataSources:really_big:AccountName" }}, Folders = new List<string> { "vectorization-input" } },
        ];

        public static List<DataSourceBase> GetDataSources()
        {
            var items = new List<DataSourceBase>();
            items.AddRange(Items);
            return items;
        }
    }
}
