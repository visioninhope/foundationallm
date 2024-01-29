using System.Text;
using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Agent.Models.Resources;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services.ResourceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FoundationaLLM.Agent.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Agent resource provider.
    /// </summary>
    public class AgentResourceProviderService(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Agent_ResourceProviderService)] IStorageService storageService,
        ILogger<AgentResourceProviderService> logger)
        : ResourceProviderServiceBase(
            storageService,
            logger)
    {
        JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        private Dictionary<string, AgentReference> _agentReferences = [];

        private const string AGENT_REFERENCES_FILE_NAME = "agent-references.json";
        private const string AGENT_FOLDER_NAME = "agents";

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Agent;

        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> _resourceTypes =>
            new()
            {
                {
                    AgentResourceTypeNames.AgentReferences,
                    new ResourceTypeDescriptor(AgentResourceTypeNames.AgentReferences)
                }
            };

        /// <inheritdoc/>
        protected override async Task InitializeInternal()
        {
            _logger.LogInformation("Starting to initialize the {ResourceProvider} resource provider...", _name);

            var agentReferencesFilePath = $"/{_name}/{AGENT_REFERENCES_FILE_NAME}";

            if (await _storageService.FileExistsAsync(_storageContainerName, agentReferencesFilePath, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, agentReferencesFilePath, default);
                var agentReferenceStore = JsonConvert.DeserializeObject<AgentReferenceStore>(
                    Encoding.UTF8.GetString(fileContent.ToArray()));

                _agentReferences = agentReferenceStore!.AgentReferences.ToDictionary(ar => ar.Name);
            }

            _logger.LogInformation("The {ResourceProvider} resource provider was successfully initialized.", _name);
        }

        /// <inheritdoc/>
        protected override async Task<T> GetResourceAsyncInternal<T>(List<ResourceTypeInstance> instances) where T: class =>
            instances[0].ResourceType switch
            {
                AgentResourceTypeNames.AgentReferences => await GetAgentReference<T>(instances),
                _ => throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.")
            };

        /// <inheritdoc/>
        protected override async Task<IList<T>> GetResourcesAsyncInternal<T>(List<ResourceTypeInstance> instances) where T : class =>
            instances[0].ResourceType switch
            {
                AgentResourceTypeNames.AgentReferences => GetAgentReferences<T>(instances),
                _ => throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.")
            };

        private List<T> GetAgentReferences<T>(List<ResourceTypeInstance> instances) where T : class
        {
            if (typeof(T) != typeof(AgentReference))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({instances[0].ResourceType}).");

            return _agentReferences.Values.Cast<T>().ToList();
        }

        private async Task<T> GetAgentReference<T>(List<ResourceTypeInstance> instances) where T : class
        {
            if (instances.Count != 1)
                throw new ResourceProviderException($"Invalid resource path");

            if (typeof(T) != typeof(AgentReference))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({instances[0].ResourceType}).");

            _agentReferences.TryGetValue(instances[0].ResourceId!, out var agentReference);
            var agent = await DeserializeAgent(agentReference!);
            return agent as T
                   ?? throw new ResourceProviderException($"The resource {instances[0].ResourceId!} of type {instances[0].ResourceType} was not found.");
        }

        /// <inheritdoc/>
        protected override async Task UpsertResourceAsync<T>(List<ResourceTypeInstance> instances, T resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            var agent = resource as AgentBase;

            var serializedAgent = SerializeAgent(agent);

            await SaveToBlobStorageAsync(agent, serializedAgent);

            await UpdateAgentIndexAsync(agent);
        }

        private async Task<AgentBase> DeserializeAgent(AgentReference agentReference)
        {
            var agentFilePath = $"/{_name}/{AGENT_FOLDER_NAME}/{agentReference.Filename}";
            var agentType = GetAgentType(agentReference);
            if (agentType == null)
                throw new ResourceProviderException($"The agent type {agentReference.Type} is not supported.");

            if (await _storageService.FileExistsAsync(_storageContainerName, agentFilePath, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, agentFilePath, default);
                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(fileContent.ToArray()),
                           agentType, settings) as AgentBase
                       ?? throw new ResourceProviderException($"Failed to deserialize the agent {agentReference.Name}.");
            }

            throw new ResourceProviderException($"Could not locate the {agentReference.Name} agent resource.");
        }

        private string SerializeAgent(AgentBase agent) =>
            JsonConvert.SerializeObject(agent, settings);

        private async Task SaveToBlobStorageAsync(AgentBase agent, string serializedAgent)
        {
            
        }

        private async Task UpdateAgentIndexAsync(AgentBase agent)
        {
            
        }

        private Type GetAgentType(AgentReference agentReference)
        {
            switch (agentReference.Type)
            {
                case AgentTypes.KnowledgeManagement:
                    return typeof(KnowledgeManagementAgent);
            }
            throw new ResourceProviderException($"The agent type {agentReference.Type} is not supported.");
        }
    }
}
