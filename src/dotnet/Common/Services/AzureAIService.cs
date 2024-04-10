using Azure.ResourceManager.EventGrid.Models;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.AzureAIService;
using FoundationaLLM.Common.Models.Configuration.AzureAI;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FoundationaLLM.Common.Services
{
    public class AzureAIService
    {
        private readonly ILogger<AzureAIService> _logger;
        private readonly IStorageService _blobStorageService;
        private readonly AzureAISettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AzureAIService(
                       IOptions<AzureAISettings> azureAISettings,
                       ILogger<AzureAIService> logger,
                       IStorageService blobStorageService,
                       IHttpClientFactory httpClientFactory)
        {
            _settings = azureAISettings.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _blobStorageService = blobStorageService;
            //_blobStorageService = new BlobStorageService(_settings.BlobStorageServiceSettings, logger); 
        }

        public async Task<string> CreateDataSet(byte[] data, string blobName)
        {
            DateTime now = DateTime.UtcNow;

            string path = $"UI/{now.ToString("yyyy-MM-dd_ffffff_UTC")}";

            //turn the byte to a stream
            Stream stream = new MemoryStream(data);

            await _blobStorageService.WriteFileAsync(_settings.ContainerName, path + $"/{blobName}.jsonl", stream, null, default);

            return $"azureml://subscriptions/{_settings.SubscriptionId}/resourcegroups/{_settings.ResourceGroup}/workspaces/{_settings.ProjectName}/datastores/workspaceblobstore/paths/{path}/{blobName}.jsonl";
        }

        public async Task<string> CreateDataSetVersion(string dataSetName, string dataSetPath, int version=1)
        {
            DatasetVersionRequest req = new DatasetVersionRequest();
            req.DataContainerName = dataSetName;
            req.DataType = "UriFile";
            req.DataUri = dataSetPath;
            req.MutableProps = new Dictionary<string, string> { { "isArchived", "false" } };
            req.IsRegistered = true;

            try
            {
                var httpClient = await CreateHttpClient();
                var response = await httpClient.PostAsync(
                    $"{_settings.BaseUrl}/api/{_settings.Region}/data/v1.0/subscriptions/{_settings.SubscriptionId}/resourceGroups/{_settings.ResourceGroup}/providers/Microsoft.MachineLearningServices/workspaces/{_settings.ProjectName}/dataversion/{dataSetName}/versions",
                    JsonContent.Create(req));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating data set version");
            }

            return null;
        }

        public async Task<Guid> SubmitJob(string displayName, string dataSetName, int dataSetVersion, string metrics)
        {
            AzureAIJobRequest job = new AzureAIJobRequest();
            job.FlowDefinitionResourceId = _settings.FlowDefinitionResourceId;
            job.RunId = Guid.NewGuid().ToString();
            job.BatchDataInput = new BatchDataInput { DataUri = $"azureml://locations/{_settings.Region}/workspaces/{job.RunId}/data/{dataSetName}/versions/{dataSetVersion}" };
            job.Connections = new GptConnections
            {
                GptCoherence = new GptConnection { Connection = "Default_AzureOpenAI", DeploymentName = _settings.Deployment },
                GptFluency = new GptConnection { Connection = "Default_AzureOpenAI", DeploymentName = _settings.Deployment },
                GptGroundedness = new GptConnection { Connection = "Default_AzureOpenAI", DeploymentName = _settings.Deployment },
                GptRelevance = new GptConnection { Connection = "Default_AzureOpenAI", DeploymentName = _settings.Deployment },
                GptSimilarity = new GptConnection { Connection = "Default_AzureOpenAI", DeploymentName = _settings.Deployment }
            };
            job.InputsMapping = new InputsMapping { Question = "${data.question}", Answer = "${data.answer}", Context = "${data.context}", GroundTruth = "${data.ground_truth}", Metrics = metrics };
            job.RunExperimentName = job.RunId;
            job.RuntimeName = "automatic";
            job.RunDisplayNameGenerationType = "UserProvidedMacro";
            job.Properties = new Dictionary<string, string> { { "runType", "eval_run" } };
            job.SessionSetupMode = "SystemWait";

            try
            {
                var options = new JsonSerializerOptions
                {
                    MaxDepth = 10
                };

                var httpClient = await CreateHttpClient();
                var response = await httpClient.PostAsync(
                    $"{_settings.BaseUrl}/api/{_settings.Region}/flow/api/subscriptions/{_settings.SubscriptionId}/resourceGroups/{_settings.ResourceGroup}/providers/Microsoft.MachineLearningServices/workspaces/{_settings.ProjectName}/BulkRuns/submit",
                    JsonContent.Create(job));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Guid.Parse(responseContent);
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error submitting job");
            }

            return Guid.Empty;
        }

        public async Task<string> GetJobStatus(Guid jobId)
        {
            try
            {
                var httpClient = await CreateHttpClient();
                var response = await httpClient.GetAsync(
                    $"{_settings.BaseUrl}/api/{_settings.Region}/history/v1.0/subscriptions/{_settings.SubscriptionId}/resourceGroups/{_settings.ResourceGroup}/providers/Microsoft.MachineLearningServices/workspaces/{_settings.ProjectName}/runs/{jobId}"
                    );

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting job status");
            }

            return null;
        }

        public async Task<string> GetResultsByIndex(Guid jobId, int startIndex=0, int endIndex=149)
        {
            try
            {
                var httpClient = await CreateHttpClient();
                var response = await httpClient.GetAsync(
                    $"{_settings.BaseUrl}/api/{_settings.Region}/flow/api/subscriptions/{_settings.SubscriptionId}/resourceGroups/{_settings.ResourceGroup}/providers/Microsoft.MachineLearningServices/workspaces/{_settings.ProjectName}/BulkRuns/{jobId}/childRuns?startIndex={startIndex}&endIndex={endIndex}"
                    );

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error downloading results by index");
            }

            return null;
        }

        public async Task<string> DownloadResults(Guid jobId)
        {
            try
            {
                var httpClient = await CreateHttpClient();
                var response = await httpClient.GetAsync(
                    $"/api/{_settings.Region}/flow/api/subscriptions/{_settings.SubscriptionId}/resourceGroups/{_settings.ResourceGroup}/providers/Microsoft.MachineLearningServices/workspaces/{_settings.ProjectName}/BulkRuns/{jobId}/results"
                    );

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error downloading results");                
            }

            return null;
        }

        private async Task<HttpClient> CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();

            //https://ai.azure.com
            httpClient.BaseAddress = new Uri(_settings.BaseUrl);

            var credentials = DefaultAuthentication.GetAzureCredential();
            var tokenResult = await credentials.GetTokenAsync(
                new(["https://management.core.windows.net/"]),
                default);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenResult.Token);

            return httpClient;
        }
    }
}
