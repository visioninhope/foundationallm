using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Tests.Models.TextEmbedding
{
    public class EmbeddedContentTests
    {
        [Fact]
        public void EmbeddedContent_Construct_ValidContent()
        {
            // Arrange
            var contentId = new ContentIdentifier
            {
                DataSourceObjectId = "/instances/1e22cd2a-7b81-4160-b79f-f6443e3a6ac2/providers/FoundationaLLM.DataSource/dataSources/datalake01",
                MultipartId = new List<string> { "part1", "part2" },
                CanonicalId = "CanonicalId"
            };

            var contentParts = new List<EmbeddedContentPart>
            {
                new EmbeddedContentPart { Id = "Part1", Content = "Content1" , Embedding = new Embedding(){ } },
                new EmbeddedContentPart { Id = "Part2", Content = "Content2", Embedding = new Embedding(){ } }
            };

            // Act
            var embeddedContent = new EmbeddedContent
            {
                ContentId = contentId,
                ContentParts = contentParts
            };

            // Assert
            Assert.Equal(contentId, embeddedContent.ContentId);
            Assert.Equal(contentParts, embeddedContent.ContentParts);
        }

    }
}
