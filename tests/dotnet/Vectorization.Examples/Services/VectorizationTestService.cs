using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.SemanticKernel.Core.Services;
using FoundationaLLM.Vectorization.Examples.Interfaces;
using FoundationaLLM.Vectorization.Examples.Setup;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Connectors.AzureAISearch;
using NSubstitute.Routing.Handlers;

#pragma warning disable SKEXP0001, SKEXP0020

namespace FoundationaLLM.Vectorization.Examples.Services
{
    /// <summary>
    /// Service for running agent conversations using the Core API.
    /// </summary>
    /// <param name="coreAPITestManager"></param>
    /// <param name="azureAIService"></param>
    public class VectorizationTestService(
        //IIndexingService indexingService,
        IManagementAPITestManager managementAPITestManager,
        IVectorizationAPITestManager vectorizationAPITestManager,
        IOptions<InstanceSettings> instanceSettings) : IVectorizationTestService
    {
        private IManagementAPITestManager _managementAPITestManager = managementAPITestManager;
        private IVectorizationAPITestManager _vectorizationAPITestManager = vectorizationAPITestManager;
        //private IIndexingService _indexService = indexingService;
        private InstanceSettings _instanceSettings = instanceSettings.Value;

        InstanceSettings IVectorizationTestService.InstanceSettings { get { return _instanceSettings; } set { _instanceSettings = value; } }

        public async Task CreateDataSource(string name, List<AppConfigurationKeyValue> configList) 
        {
            foreach(var config in configList)
                await _managementAPITestManager.CreateAppConfiguration(config);

            await managementAPITestManager.CreateDataSource(name);
        }

        public Task CreateContentSourceProfile(string name)
        {
            return managementAPITestManager.CreateContentSourceProfile(name);
        }

        public Task CreateTextPartitioningProfile(string name)
        {
            return managementAPITestManager.CreateTextPartitioningProfile(name);
        }

        public Task CreateTextEmbeddingProfile(string name)
        {
            return managementAPITestManager.CreateTextEmbeddingProfile(name);
        }

        public Task CreateIndexingProfile(string name)
        {
            return managementAPITestManager.CreateIndexingProfile(name);
        }

        public Task<VectorizationResult> CreateVectorizationRequest(VectorizationRequest request)
        {
            return _vectorizationAPITestManager.CreateVectorizationRequest(request);
        }

        public Task<VectorizationRequest> CheckVectorizationRequestStatus(VectorizationRequest request)
        {
            return managementAPITestManager.GetVectorizationRequest(request);

        }

        async public Task<string> QueryIndex(string indexProfileName, string embedProfileName, string query)
        {
            //get the indexing profile
            IndexingProfile indexingProfile = await managementAPITestManager.GetIndexingProfile(indexProfileName);

            TextEmbeddingProfile embeddingProfile = await managementAPITestManager.GetTextEmbeddingProfile(embedProfileName);

            return await QueryIndex(indexingProfile, embeddingProfile, query);
        }

        public async Task<string> QueryIndex(IndexingProfile indexProfile, TextEmbeddingProfile embedProfile, string query)
        {
            string searchServiceEndPoint = await TestConfiguration.GetAppConfigValueAsync( indexProfile.ConfigurationReferences["Endpoint"]);
            string authType = await TestConfiguration.GetAppConfigValueAsync(indexProfile.ConfigurationReferences["AuthenticationType"]);            

            SearchIndexClient indexClient = null;

            switch(authType)
            {
                case "AzureIdentity":
                    indexClient = new SearchIndexClient(new Uri(searchServiceEndPoint), new DefaultAzureCredential());
                    break;
                case "ApiKey":
                    string adminApiKey = await TestConfiguration.GetAppConfigValueAsync(indexProfile.ConfigurationReferences["ApiKey"]);
                    indexClient = new SearchIndexClient(new Uri(searchServiceEndPoint), new AzureKeyCredential(adminApiKey));
                    break;

            }
            
            var searchClient = indexClient.GetSearchClient(indexProfile.Settings["IndexName"]);

            //Do basic search...
            SearchResults<object> sr = searchClient.Search<object>(query);

            //embed the query
            string oaiEndpoint = await TestConfiguration.GetAppConfigValueAsync(embedProfile.ConfigurationReferences["Endpoint"]);
            authType = await TestConfiguration.GetAppConfigValueAsync(embedProfile.ConfigurationReferences["AuthenticationType"]);
            string apiVersion = await TestConfiguration.GetAppConfigValueAsync(embedProfile.ConfigurationReferences["APIVersion"]);
            AzureKeyCredential credentials = new(await TestConfiguration.GetAppConfigValueAsync(embedProfile.ConfigurationReferences["APIKey"]));

            OpenAIClient openAIClient = new(new Uri(oaiEndpoint), credentials);

            EmbeddingsOptions embeddingOptions = new()
            {
                DeploymentName = await TestConfiguration.GetAppConfigValueAsync(embedProfile.ConfigurationReferences["DeploymentName"]),
                Input = { query },
            };

            var returnValue = openAIClient.GetEmbeddings(embeddingOptions);

            var vectors = returnValue.Value.Data[0].Embedding.ToArray();

            //Do Vector Search
            //TODO
            
            return "TODO";
        }

        public async Task DeleteDataSource(string name, List<AppConfigurationKeyValue> configList)
        {
            await managementAPITestManager.DeleteDataSource(name, configList);

            return;
        }

        public Task DeleteContentSourceProfile(string name)
        {
            return managementAPITestManager.DeleteContentSourceProfile(name);
        }

        public Task DeleteTextPartitioningProfile(string name)
        {
            return managementAPITestManager.DeleteTextPartitioningProfile(name);
        }

        public Task DeleteTextEmbeddingProfile(string name)
        {
            return managementAPITestManager.DeleteTextEmbeddingProfile(name);
        }


        public Task DeleteIndexingProfile(string name, bool deleteIndex = true)
        {
            return managementAPITestManager.DeleteIndexingProfile(name);
        }

        public Task DeleteVectorizationRequest(VectorizationRequest request)
        {
            //remove all artifacts related to the request
            return null;
        }
    }
}
