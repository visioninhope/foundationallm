using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Agents;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Catalogs;
using FoundationaLLM.Core.Examples.Exceptions;
using FoundationaLLM.Core.Examples.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class ManagementAPITestManager(
        IHttpClientManager httpClientManager,
        IOptions<InstanceSettings> instanceSettings) : IManagementAPITestManager
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        public async Task CreateDataSource(IStorageService svc, string dataSourceName)
        {
            var ds = DataSourceCatalog.GetDataSources().FirstOrDefault(a => a.Name == dataSourceName);

            //upload the dune file...
            var azureDataLakeDataSourceObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_DataSource,
                $"datasources/{dataSourceName}",
                ds);
        }

        public async Task CreateIndexingProfile(string indexingProfileName)
        {
            var indexingProfile = IndexingProfilesCatalog.GetIndexingProfiles().FirstOrDefault(a => a.Name == indexingProfileName);

            var indexingProfileObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Vectorization,
                $"indexingprofiles/{indexingProfileName}",
                indexingProfile);
        }

        public async Task CreateTextEmbeddingProfile(string textEmbeddingProfileName)
        {
            var textEmbeddingProfile = TextEmbeddingProfileCatalog.GetTextEmbeddingProfiles().FirstOrDefault(a => a.Name == textEmbeddingProfileName);

            var textEmbeddingProfileObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Vectorization,
                $"textembeddingprofiles/{textEmbeddingProfileName}",
                textEmbeddingProfile);
        }

        public async Task CreateTextPartitioningProfile(string textPartitioningProfileName)
        {
            var textPartitioningProfile = TextPartitioningProfileCatalog.GetTextPartitioningProfiles().FirstOrDefault(a => a.Name == textPartitioningProfileName);

            var textPartitioningProfileObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Vectorization,
                $"textpartitioningprofiles/{textPartitioningProfileName}",
                textPartitioningProfile);
        }

            public async Task CreateContentSourceProfile(string profileName)
        {
            var contentSourceProfile = DataSourceCatalog.GetDataSources().FirstOrDefault(a => a.Name == profileName);

            var textPartitioningProfileObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_DataSource,
                $"contentsource/{profileName}",
                contentSourceProfile);
        }

        public async Task DeleteDataSource(string profileName)
        {
            return;
        }

        public async Task DeleteContentSourceProfile(string profileName)
        {
            await DeleteResourceAsync(
                               instanceSettings.Value.Id,
                                              ResourceProviderNames.FoundationaLLM_DataSource,
                                                             $"contentsource/{profileName}");
        }

        public async Task DeleteTextPartitioningProfile(string profileName)
        {
            await DeleteResourceAsync(
                                              instanceSettings.Value.Id,
                                                                                           ResourceProviderNames.FoundationaLLM_Vectorization,
                                                                                                                                                       $"textpartitioningprofiles/{profileName}");
        }

        public async Task DeleteIndexingProfile(string profileName)
        {
            await DeleteResourceAsync(
                                                             instanceSettings.Value.Id,
                                                                                                                                                       ResourceProviderNames.FoundationaLLM_Vectorization,
                                                                                                                                                                                                                                                                                                             $"indexingprofiles/{profileName}");
        }

        public async Task DeleteTextEmbeddingProfile(string profileName)
        {
            await DeleteResourceAsync(
                                                                                                                                                                      instanceSettings.Value.Id,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ResourceProviderNames.FoundationaLLM_Vectorization,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        $"textembeddingprofiles/{profileName}");
        }

        /// <inheritdoc/>
        public async Task<AgentBase> CreateAgent(string agentName)
        {
            // All test agents should have a corresponding prompt in the catalog.
            // Retrieve the agent and prompt from the test catalog.
            var agent = AgentCatalog.GetAllAgents().FirstOrDefault(a => a.Name == agentName);
            var prompt = PromptCatalog.GetAllPrompts().FirstOrDefault(p => p.Name == agentName);
            
            if (agent == null)
            {
                throw new InvalidOperationException($"The agent {agentName} was not found.");
            }
            if (prompt == null)
            {
                throw new InvalidOperationException($"The prompt for the agent {agentName} was not found.");
            }

            // Create the prompt for the agent.
            var promptObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Prompt,
                $"prompts/{agentName}",
                prompt);

            // Add the prompt ObjectId to the agent.
            agent.PromptObjectId = promptObjectId;

            // TODO: Create any other dependencies for the agent here.

            // Create the agent.
            var agentObjectId = await UpsertResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Agent,
                $"agents/{agentName}",
                agent);

            agent.ObjectId = agentObjectId;

            return agent;
        }

        /// <inheritdoc/>
        public async Task DeleteAgent(string agentName)
        {
            // Delete the agent and its dependencies.

            // Delete the agent's prompt.
            await DeleteResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Prompt,
                $"prompts/{agentName}");

            // TODO: Delete other dependencies for the agent here.

            // Delete the agent.
            await DeleteResourceAsync(
                instanceSettings.Value.Id,
                ResourceProviderNames.FoundationaLLM_Agent,
                $"agents/{agentName}");
        }

        /// <inheritdoc/>
        public async Task<object?> GetResourcesAsync(string instanceId, string resourceProvider, string resourcePath)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.ManagementAPI);
            var response = await coreClient.GetAsync($"instances/{instanceId}/providers/{resourceProvider}/{resourcePath}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var resources = JsonSerializer.Deserialize<object>(responseContent, _jsonSerializerOptions);
                return resources;
            }

            throw new FoundationaLLMException($"Failed to get resources. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task<string> UpsertResourceAsync(string instanceId, string resourceProvider, string resourcePath,
            object resource)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.ManagementAPI);
            var serializedRequest = JsonSerializer.Serialize(resource, _jsonSerializerOptions);

            var response = await coreClient.PostAsync($"instances/{instanceId}/providers/{resourceProvider}/{resourcePath}",
                               new StringContent(serializedRequest, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var upsertResult = JsonSerializer.Deserialize<ResourceProviderUpsertResult>(responseContent, _jsonSerializerOptions);
                if (upsertResult != null)
                    return upsertResult.ObjectId ??
                           throw new InvalidOperationException("The returned object ID is invalid.");
            }

            throw new FoundationaLLMException($"Failed to upsert resource. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        /// <inheritdoc/>
        public async Task DeleteResourceAsync(string instanceId, string resourceProvider, string resourcePath)
        {
            var coreClient = await httpClientManager.GetHttpClientAsync(HttpClients.ManagementAPI);
            var response = await coreClient.DeleteAsync($"instances/{instanceId}/providers/{resourceProvider}/{resourcePath}");

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new FoundationaLLMException($"Failed to delete resource. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }
    }
}
