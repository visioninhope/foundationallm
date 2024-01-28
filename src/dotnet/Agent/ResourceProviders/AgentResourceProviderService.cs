using System.Text;
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
        private Dictionary<string, AgentReference> _agentReferences = [];

        private const string AGENT_REFERENCES_FILE_NAME = "agent-references.json";

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
        protected override T GetResourceInternal<T>(List<ResourceTypeInstance> instances) where T: class =>
            instances[0].ResourceType switch
            {
                AgentResourceTypeNames.AgentReferences => GetAgentReferences<T>(instances),
                _ => throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.")
            };

        private T GetAgentReferences<T>(List<ResourceTypeInstance> instances) where T : class
        {
            if (instances.Count != 1)
                throw new ResourceProviderException($"Invalid resource path");

            if (typeof(T) != typeof(AgentReference))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({instances[0].ResourceType}).");

            _agentReferences.TryGetValue(instances[0].ResourceId!, out var agentReference);
            return agentReference as T
                ?? throw new ResourceProviderException($"The resource {instances[0].ResourceId!} of type {instances[0].ResourceType} was not found.");
        }

        /// <inheritdoc/>
        protected override async Task UpsertResourceAsync<T>(List<ResourceTypeInstance> instances, T resource)
        {
            // TODO: Complete this method.
        }
    }
}
