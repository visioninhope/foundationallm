using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.TextEmbedding;
using FoundationaLLM.Vectorization.Services.DataSources;
using Microsoft.Extensions.Logging;

namespace Vectorization.Tests.Services.DataSources
{
    public class DataLakeDataSourceServiceTests
    {
        private DataLakeDataSourceService _dataLakeContentSourceService;
        
        public DataLakeDataSourceServiceTests() {
            _dataLakeContentSourceService = new DataLakeDataSourceService(
                new BlobStorageServiceSettings { 
                    AuthenticationType = BlobStorageAuthenticationTypes.ConnectionString,
                    ConnectionString = Environment.GetEnvironmentVariable("DataLakeContentServiceTestsConnectionString")
                },
                LoggerFactory.Create(builder => builder.AddConsole())
            );
        }
        
        [Fact]
        public async void TestExtractTextFromFile()
        {
            // TXT
            Assert.Equal(
                "This is a test string in the Vectorization Data Lake.",
                await _dataLakeContentSourceService.ExtractTextAsync(
                    new ContentIdentifier
                    {
                        CanonicalId = "vectorization-content-test.txt",
                        DataSourceObjectId = "/instances/1e22cd2a-7b81-4160-b79f-f6443e3a6ac2/providers/FoundationaLLM.DataSource/dataSources/datalake01",
                        MultipartId = new List<string> {
                            Environment.GetEnvironmentVariable("DataLakeContentServiceTestsContainerUrl"),
                            "testing",
                            "vectorization-content-test.txt"
                        }
                    },
                    new CancellationTokenSource().Token
                )
            );

            // DOCX
            Assert.Equal(
                "This is a test string in the Vectorization Data Lake.",
                await _dataLakeContentSourceService.ExtractTextAsync(
                    new ContentIdentifier
                    {
                        CanonicalId = "vectorization-content-test.docx",
                        DataSourceObjectId = "/instances/1e22cd2a-7b81-4160-b79f-f6443e3a6ac2/providers/FoundationaLLM.DataSource/dataSources/datalake01",
                        MultipartId = new List<string> {
                            Environment.GetEnvironmentVariable("DataLakeContentServiceTestsContainerUrl"),
                            "testing",
                            "vectorization-content-test.docx"
                        }
                    },
                    new CancellationTokenSource().Token
                )
            );
        }
    }
}
