using FluentValidation;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Core.Examples.Setup;
using FoundationaLLM.Vectorization.Examples.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private string contentSourceProfileName = "content_source_profile";


		[Fact]
		public async Task RunAsync()
		{
            await PreExecute();

			await RunExampleAsync();

            await PostExecute();
		}

        private async Task PreExecute()
        {
            string duneArtifactPath = "https://www.dune.com/dune.pdf";
            string containerName = "dune";

            //download the file via http client
            //HttpClientService svc = ServiceProvider.GetService<HttpClientService>();
            //await svc.DownloadFileAsync(duneArtifactPath, "dune.pdf");
            byte[] data;

            //upload the dune artifact to storage
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(duneArtifactPath))
                data = result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;

            //try byte array into stream
            var stream = new MemoryStream(data);

            BlobStorageService svc = ServiceProvider.GetService<BlobStorageService>();
            await svc.WriteFileAsync(containerName, "dune.pdf", stream, null, default);

            //create the data source
            _vectorizationTestService.CreateDataSource(svc, "dune");

            //content source profile
            _vectorizationTestService.CreateContentSourceProfile("");

            //text partitioning profile
            _vectorizationTestService.CreateTextPartitioningProfile(textPartitionProfileName);

            //text embedding profile
            _vectorizationTestService.CreateTextEmbeddingProfile(textEmbeddingProfileName);

            //indexing profile
            _vectorizationTestService.CreateIndexingProfile(indexingProfileName);
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
            _vectorizationTestService.DeleteDataSource("dune");

            //remove content source profile
            //content source profile
            _vectorizationTestService.DeleteContentSourceProfile(contentSourceProfileName);

            //text partitioning profile
            //remove text partitioning profile
            _vectorizationTestService.DeleteTextPartitioningProfile(textPartitionProfileName);

            //text embedding profile
            //remove text embedding profile
            _vectorizationTestService.DeleteTextEmbeddingProfile(textEmbeddingProfileName);

            //indexing profile
            //remove search index
            //remove indexing profile
            _vectorizationTestService.DeleteIndexingProfile(indexingProfileName, true);
        }
	}
}
