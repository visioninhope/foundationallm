using Azure.Search.Documents.Models;

namespace FoundationaLLM.Core.Examples.Models
{
    public class TestSearchResult
    {
        public string Query { get; set; }

        public SearchResults<object> VectorResults { get; set; }

        public SearchResults<object> QueryResult { get; set; }
    }
}
