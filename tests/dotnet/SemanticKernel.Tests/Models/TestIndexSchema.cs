using Azure.Search.Documents.Indexes;

namespace SemanticKernel.Tests.Models
{
    public class TestIndexSchema
    {
        [SimpleField(IsKey = true)]
        public string Id { get; set; }

        [VectorSearchField(
            VectorSearchDimensions = 1536,
            VectorSearchProfileName = "vector-config"
        )]
        public float[] Embedding { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Text { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string Description { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string AdditionalMetadata { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string ExternalSourceName { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public bool IsReference { get; set; }
    }
}
