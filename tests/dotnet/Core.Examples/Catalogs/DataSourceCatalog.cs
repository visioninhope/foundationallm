using FoundationaLLM.Common.Models.ResourceProviders.DataSource;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class DataSourceCatalog
    {
        public static readonly List<DataSourceBase> Items =
        [

        ];

        public static List<DataSourceBase> GetDataSources()
        {
            var items = new List<DataSourceBase>();
            items.AddRange(Items);
            return items;
        }
    }
}
