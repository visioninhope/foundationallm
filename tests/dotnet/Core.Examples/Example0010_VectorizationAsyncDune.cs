using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using FoundationaLLM.Core.Examples.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running the default FoundationaLLM agent completions in both session and sessionless modes.
    /// </summary>
    public class Example0010_VectorizationAsyncDune : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IVectorizationTestService _vectorizationTestService;
        private BlobStorageService _svc;
        private InstanceSettings _instanceSettings;

        public Example0010_VectorizationAsyncDune(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _vectorizationTestService = GetService<IVectorizationTestService>();
            _instanceSettings = _vectorizationTestService.InstanceSettings;

            dataSourceObjectId = $"/instances/{_instanceSettings.Id}/providers/FoundationaLLM.DataSource/dataSources/{contentSourceProfileName}";
    }

        private string textPartitionProfileName = "text_partition_profile";
        private string textEmbeddingProfileName = "text_embedding_profile_gateway";
        private string indexingProfileName = "indexing_profile";

        private string genericTextEmbeddingProfileName = "text_embedding_profile_generic";

        private string contentSourceProfileName = "really_big";
        private string containerName = "data";
        private string blobName = "really_big.pdf";
        private string dataSourceObjectId;
        private string id = String.Empty;
        private List<AppConfigurationKeyValue> configValues = new List<AppConfigurationKeyValue>();
        private VectorizationRequest request;


        [Fact]
		public async Task RunAsync()
		{
            await PreExecute();

			await RunExampleAsync();

            await PostExecute();
		}

        private async Task PreExecute()
        {
            id = Guid.NewGuid().ToString();

            var settings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);
                       
            var logger = ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>();

            _svc = new BlobStorageService(
                Options.Create<BlobStorageServiceSettings>(settings),
                logger)
            {
                InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization
            };

            string artifactPath = "https://solliancepublicdata.blob.core.windows.net/data/data/really_big.pdf";

            byte[] data;

            //upload the dune artifact to storage
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(artifactPath))
                data = result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;

            await _svc.CreateContainerAsync(containerName);

            //try byte array into stream
            var stream = new MemoryStream(data);
            await _svc.WriteFileAsync(containerName, blobName, stream, null, default);

            //create the data source
            AppConfigurationKeyValue appConfigurationKeyValue = new AppConfigurationKeyValue { Name = contentSourceProfileName };
            appConfigurationKeyValue.Key = $"FoundationaLLM:DataSources:{contentSourceProfileName}:AuthenticationType";
            appConfigurationKeyValue.Value = settings.AuthenticationType.ToString();
            appConfigurationKeyValue.ContentType = "";

            configValues.Add(appConfigurationKeyValue);

            appConfigurationKeyValue = new AppConfigurationKeyValue { Name = contentSourceProfileName };
            appConfigurationKeyValue.Key = $"FoundationaLLM:DataSources:{contentSourceProfileName}:AccountName";
            appConfigurationKeyValue.Value = settings.AccountName;
            appConfigurationKeyValue.ContentType = "";

            configValues.Add(appConfigurationKeyValue);

            await _vectorizationTestService.CreateDataSource(contentSourceProfileName, configValues);                      

            //text partitioning profile
            await _vectorizationTestService.CreateTextPartitioningProfile(textPartitionProfileName);

            //gateway text embedding profile
            await _vectorizationTestService.CreateTextEmbeddingProfile(textEmbeddingProfileName);
            
            //generic text embedding profile
            await _vectorizationTestService.CreateTextEmbeddingProfile(genericTextEmbeddingProfileName);

            //indexing profile
            await _vectorizationTestService.CreateIndexingProfile(indexingProfileName);
        }

        private async Task RunExampleAsync()
        {
            string containerPath = $"https://{_svc.BlobServiceClient.AccountName}.blob.core.windows.net";

            ContentIdentifier ci = new ContentIdentifier
            {
                DataSourceObjectId = dataSourceObjectId,                
                MultipartId = new List<string>
                {
                    containerPath,
                    containerName,
                    blobName
                },
                CanonicalId = containerName + "/" + blobName.Substring(0, blobName.LastIndexOf('.'))
            };

            //start a vectorization request...
            List<VectorizationStep> steps =
            [
                new VectorizationStep { Id = "extract", Parameters = new Dictionary<string, string>() },
                new VectorizationStep { Id = "partition", Parameters = new Dictionary<string, string>() { { "text_partitioning_profile_name", textPartitionProfileName } } },
                new VectorizationStep { Id = "embed", Parameters = new Dictionary<string, string>() { { "text_embedding_profile_name", textEmbeddingProfileName } } },
                new VectorizationStep { Id = "index", Parameters = new Dictionary<string, string>() { { "indexing_profile_name", indexingProfileName } } },
            ];

            //Create a vectorization request.
            request = new VectorizationRequest
            {
                RemainingSteps = new List<string> { "extract", "partition", "embed", "index" },
                CompletedSteps = new List<string>(),
                ProcessingType = VectorizationProcessingType.Asynchronous,
                ContentIdentifier = ci,
                Id = id,
                Steps = steps,
                ObjectId = $"{VectorizationResourceTypeNames.VectorizationRequests}/{id}"
            };

            //Create the vectorization request, re-assign the fully qualified object id if desired.
            request.ObjectId = await _vectorizationTestService.CreateVectorizationRequest(request);

            //Issue the process action on the vectorization request
            var vectorizationResult = await _vectorizationTestService.ProcessVectorizationRequest(request);

            // Ensure the vectorization request was successful
            if (vectorizationResult == null)
                throw new Exception("Vectorization request failed to complete successfully. Invalid result was returned.");
                
            if(vectorizationResult.IsSuccess == false)
                throw new Exception($"Vectorization request failed to complete successfully. Message: {vectorizationResult.ErrorMessage}");

            //As this is an asynchronous request, poll the status of the vectorization request until it has completed (or failed). Retrieve initial state.
            VectorizationRequest resource = _vectorizationTestService.CheckVectorizationRequestStatus(request).Result;

            // The finalized state of the vectorization request is either "Completed" or "Failed"
            // Give it a max of 10 minutes to complete, then exit loop and fail the test.
            int timeRemainingMilliseconds = 600000;
            var pollDurationMilliseconds = 30000; //poll duration of 30 seconds
            while (resource.ProcessingState != VectorizationProcessingState.Completed && resource.ProcessingState != VectorizationProcessingState.Failed && timeRemainingMilliseconds > 0)
            {                
                Thread.Sleep(pollDurationMilliseconds);                
                timeRemainingMilliseconds -= pollDurationMilliseconds;
                resource = await _vectorizationTestService.CheckVectorizationRequestStatus(request);
            }

            if (resource.ProcessingState == VectorizationProcessingState.Failed)
                throw new Exception($"Vectorization request failed to complete successfully. {string.Join(",",resource.ErrorMessages)}");

            if (timeRemainingMilliseconds <=0)
                throw new Exception("Vectorization request failed to complete successfully. Timeout exceeded.");           

            /*
            //verify artifacts
            //TODO

            //perform a search
            TestSearchResult result = await _vectorizationTestService.QueryIndex(indexingProfileName, genericTextEmbeddingProfileName, "dune");

            //verify expected results
            if (result.VectorResults.TotalCount != 281)
                throw new Exception("Expected 281 vector results, but got " + result.VectorResults.TotalCount);

            //vaidate chunks in index...
            if ( result.QueryResult.TotalCount != 2886)
                throw new Exception("Expected 2883 search results, but got " + result.QueryResult.TotalCount);
            */
            

        }

        private async Task PostExecute()
        {
            //remove vectorization artifacts

            //remove the dune artifact from storage
            _svc.BlobServiceClient.GetBlobContainerClient(containerName).DeleteBlobIfExists(blobName);
            
            /*
            //remove the data source
            await _vectorizationTestService.DeleteDataSource(contentSourceProfileName, configValues);        

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

            //indexing profile
            //remove search index
            //remove indexing profile
            await _vectorizationTestService.DeleteVectorizationRequest(request);
            */
        }
	}
}
