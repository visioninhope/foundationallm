using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Tests.Models.TextEmbedding
{
    public class ContentIdentifierTests
    {
        private ContentIdentifier _identifier = new ContentIdentifier
        {
            DataSourceObjectId = "/instances/1e22cd2a-7b81-4160-b79f-f6443e3a6ac2/providers/FoundationaLLM.DataSource/dataSources/datalake01",
            MultipartId = new List<string> { "part1", "part2" },
            CanonicalId = "CanonicalId"
        };

        [Fact]
        public void ContentIdentifier_Construct_ValidContentIdentifier()
        {
            // Arrange
            List<string> multipartId = new List<string> { "part1", "part2" };

            // Assert
            Assert.Equal("ProfileName", _identifier.DataSourceObjectId);
            Assert.Equal("CanonicalId", _identifier.CanonicalId);
            Assert.Equal(string.Join("/", multipartId), _identifier.UniqueId);
            Assert.Equal(multipartId.Last(), _identifier.FileName);
        }

        [Fact]
        public void ContentIdentifier_ValidateMultipartId_ValidPartsCount()
        {
            // Act
            _identifier.ValidateMultipartId(2); // Should not throw any exceptions
        }

        [Fact]
        public void ContentIdentifier_ValidateMultipartId_InvalidPartsCount()
        {
            // Act
            Assert.Throws<ContentIdentifierException>(() => _identifier.ValidateMultipartId(3));
        }

        [Fact]
        public void ContentIdentifier_Indexer_InvalidIndex()
        {
            // Act & Assert
            Assert.Throws<ContentIdentifierException>(() => { var component = _identifier[2]; });
        }
    }
}
