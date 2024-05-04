using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Vectorization.Examples.Interfaces;
using FoundationaLLM.Vectorization.Examples.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running the default FoundationaLLM agent completions in both session and sessionless modes.
    /// </summary>
    public class Example0001_VectorizationAsyncDune : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IVectorizationTestService _vectorizationTestService;

		public Example0001_VectorizationAsyncDune(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _vectorizationTestService = GetService<IVectorizationTestService>();
		}

        private string textPartitionProfileName = "text_partition_profile";
        private string textEmbeddingProfileName = "text_embedding_profile";
        private string indexingProfileName = "indexing_profile";
        private string contentSourceProfileName = "really_big";


		[Fact]
		public async Task RunAsync()
		{
            await PreExecute();

			await RunExampleAsync();

            await PostExecute();
		}

        private async Task PreExecute()
        {
            var settings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);
            
            var logger = ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>();

            BlobStorageService svc = new BlobStorageService(
                Options.Create<BlobStorageServiceSettings>(settings),
                logger)
            {
                InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization
            };

            string artifactPath = "https://solliancepublicdata.blob.core.windows.net/data/data/really_big.pdf";
            string containerName = "data";

            byte[] data;

            //upload the dune artifact to storage
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(artifactPath))
                data = result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;

            //try byte array into stream
            var stream = new MemoryStream(data);
            await svc.WriteFileAsync(containerName, "really_big.pdf", stream, null, default);

            //create the data source
            await _vectorizationTestService.CreateDataSource(svc, contentSourceProfileName);

            //content source profile
            await _vectorizationTestService.CreateContentSourceProfile(contentSourceProfileName);

            //text partitioning profile
            await _vectorizationTestService.CreateTextPartitioningProfile(textPartitionProfileName);

            //text embedding profile
            await _vectorizationTestService.CreateTextEmbeddingProfile(textEmbeddingProfileName);

            //indexing profile
            await _vectorizationTestService.CreateIndexingProfile(indexingProfileName);
        }

        private async Task RunExampleAsync()
        {
            ContentIdentifier ci = null;
            string dataSourceObjectId = null;
            string id = Guid.NewGuid().ToString();

            //start a vectorization request...
            List<VectorizationStep> steps = new List<VectorizationStep>();
            steps.Add(new VectorizationStep { Id = "extract", Parameters = new Dictionary<string, string>() });
            steps.Add(new VectorizationStep { Id = "partition", Parameters = new Dictionary<string, string>() { { "text_partition_profile_name", textPartitionProfileName } } });
            steps.Add(new VectorizationStep { Id = "embed", Parameters = new Dictionary<string, string>() { { "text_embedding_profile_name", textEmbeddingProfileName } } });
            steps.Add(new VectorizationStep { Id = "index", Parameters = new Dictionary<string, string>() { { "indexing_profile_name", indexingProfileName } } });

            //Create a vectorization request.
            var vectorizationRequest = new VectorizationRequest
            {
                RemainingSteps = new List<string> { "extract", "partition", "embed", "index" },
                CompletedSteps = new List<string>(),
                ProcessingType = VectorizationProcessingType.Asynchronous,
                ContentIdentifier = ci,
                Id = id,
                Steps = steps,
                ObjectId = dataSourceObjectId
            };

            //Add the steps to the vectorization request.
            await _vectorizationTestService.CreateVectorizationRequest(vectorizationRequest);

            //check the status of the vectorization request
            await _vectorizationTestService.CheckVectorizationRequestStatus(vectorizationRequest);

            //verify artifacts

            //perform a search
            string query = "Dune";
            
            await _vectorizationTestService.QueryIndex(indexingProfileName, query);

            //verify expected results

        }

        private async Task PostExecute()
        {
            //remove vectorization artifacts

            //remove the dune artifact from storage

            //remove the data source
            await _vectorizationTestService.DeleteDataSource("dune");

            //remove content source profile
            //content source profile
            await _vectorizationTestService.DeleteContentSourceProfile(contentSourceProfileName);

            //text partitioning profile
            //remove text partitioning profile
            await _vectorizationTestService.DeleteTextPartitioningProfile(textPartitionProfileName);

            //text embedding profile
            //remove text embedding profile
            await _vectorizationTestService.DeleteTextEmbeddingProfile(textEmbeddingProfileName);

            //indexing profile
            //remove search index
            //remove indexing profile
            await _vectorizationTestService.DeleteIndexingProfile(indexingProfileName, true);
        }
	}
}
