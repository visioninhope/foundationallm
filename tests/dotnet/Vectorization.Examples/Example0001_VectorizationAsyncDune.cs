using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Vectorization.Examples.Interfaces;
using FoundationaLLM.Vectorization.Examples.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using System.IO;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running the default FoundationaLLM agent completions in both session and sessionless modes.
    /// </summary>
    public class Example0001_VectorizationAsyncDune : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IVectorizationTestService _vectorizationTestService;
        private BlobStorageService _svc;
        private InstanceSettings _instanceSettings;

        public Example0001_VectorizationAsyncDune(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _vectorizationTestService = GetService<IVectorizationTestService>();
            _instanceSettings = _vectorizationTestService.InstanceSettings;

            dataSourceObjectId = $"/instances/{_instanceSettings.Id}/providers/FoundationaLLM.DataSource/dataSources/{contentSourceProfileName}";
    }

        private string textPartitionProfileName = "text_partition_profile";
        private string textEmbeddingProfileName = "text_embedding_profile";
        private string indexingProfileName = "indexing_profile";
        private string contentSourceProfileName = "really_big";
        private string containerName = "data";
        private string blobName = "really_big.pdf";
        private string dataSourceObjectId;
        private string id = "15b799fc-1498-497e-a7f9-7231af56abc6";
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
            var settings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);

            settings.AccountName = "st63hvhar5z5zz2";
            
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
            appConfigurationKeyValue.Key = $"FoundationaLLM:DataSources:{contentSourceProfileName}:ConnectionString";
            appConfigurationKeyValue.Value = settings.ConnectionString;
            appConfigurationKeyValue.ContentType = "";

            configValues.Add(appConfigurationKeyValue);

            await _vectorizationTestService.CreateDataSource(contentSourceProfileName, configValues);

            //content source profile
            //await _vectorizationTestService.CreateContentSourceProfile(contentSourceProfileName);

            //text partitioning profile
            await _vectorizationTestService.CreateTextPartitioningProfile(textPartitionProfileName);

            //text embedding profile
            await _vectorizationTestService.CreateTextEmbeddingProfile(textEmbeddingProfileName);

            //indexing profile
            await _vectorizationTestService.CreateIndexingProfile(indexingProfileName);
        }

        private async Task RunExampleAsync()
        {
            string containerPath = $"https://{_svc.BlobServiceClient.AccountName}.blob.core.windows.net/{containerName}";

            ContentIdentifier ci = new ContentIdentifier
            {
                DataSourceObjectId = dataSourceObjectId,
                //ContentSourceProfileName = _crawlerSettings.ContentSourceProfileName,
                MultipartId = new List<string>
                {
                    containerPath,
                    containerName,
                    blobName
                     },
                CanonicalId = id.ToString()
            };

            //start a vectorization request...
            List<VectorizationStep> steps = new List<VectorizationStep>();
            steps.Add(new VectorizationStep { Id = "extract", Parameters = new Dictionary<string, string>() });
            steps.Add(new VectorizationStep { Id = "partition", Parameters = new Dictionary<string, string>() { { "text_partitioning_profile_name", textPartitionProfileName } } });
            steps.Add(new VectorizationStep { Id = "embed", Parameters = new Dictionary<string, string>() { { "text_embedding_profile_name", textEmbeddingProfileName } } });
            steps.Add(new VectorizationStep { Id = "index", Parameters = new Dictionary<string, string>() { { "indexing_profile_name", indexingProfileName } } });

            //Create a vectorization request.
            request = new VectorizationRequest
            {
                RemainingSteps = new List<string> { "extract", "partition", "embed", "index" },
                CompletedSteps = new List<string>(),
                ProcessingType = VectorizationProcessingType.Asynchronous,
                ContentIdentifier = ci,
                Id = id,
                Steps = steps,
                //ObjectId = dataSourceObjectId
                ObjectId = $"{VectorizationResourceTypeNames.VectorizationRequests}/{id}"
            };

            //Add the steps to the vectorization request.
            var vectorizationResponse = await _vectorizationTestService.CreateVectorizationRequest(request);

            //check the status of the vectorization request
            VectorizationRequest state = _vectorizationTestService.CheckVectorizationRequestStatus(request).Result;

            while (state.ProcessingState != VectorizationProcessingState.Completed)
            {
                state = await _vectorizationTestService.CheckVectorizationRequestStatus(request);

                Thread.Sleep(1000);
            }

            //verify artifacts
            //TODO

            //perform a search
            string query = "Dune";

            //verify expected results
            await _vectorizationTestService.QueryIndex(indexingProfileName, query);

            //vaidate chunks in index...
            //TODO
        }

        private async Task PostExecute()
        {
            //remove vectorization artifacts

            //remove the dune artifact from storage
            //TODO - await _svc.DeleteFileAsync(containerName, blobName, default);

            //remove the data source
            await _vectorizationTestService.DeleteDataSource(contentSourceProfileName, configValues);

            //remove content source profile
            //content source profile
            //await _vectorizationTestService.DeleteContentSourceProfile(contentSourceProfileName);

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
        }
	}
}
