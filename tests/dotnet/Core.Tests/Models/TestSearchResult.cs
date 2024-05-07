using Azure.Search.Documents.Models;

namespace FoundationaLLM.Core.Tests.Models
{
    public class TestSearchResult
    {
        public string Query { get; set; }

        public SearchResults<object> VectorResults { get; set; }

        public SearchResults<object> QueryResult { get; set; }
    }
}
