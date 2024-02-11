﻿using System.Collections.Concurrent;
using System.Text;
using Azure.Messaging;
using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Agent.Models.Resources;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Events;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Common.Services.ResourceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FoundationaLLM.Agent.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Agent resource provider.
    /// </summary>
    public class AgentResourceProviderService(
        IOptions<InstanceSettings> instanceOptions,
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Agent)] IStorageService storageService,
        IEventService eventService,
        ILoggerFactory loggerFactory)
        : ResourceProviderServiceBase(
            instanceOptions.Value,
            storageService,
            eventService,
            loggerFactory.CreateLogger<AgentResourceProviderService>(),
            [
                EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Agent
            ])
    {
        private ConcurrentDictionary<string, AgentReference> _agentReferences = [];

        private const string AGENT_REFERENCES_FILE_NAME = "_agent-references.json";
        private const string AGENT_REFERENCES_FILE_PATH = $"/{ResourceProviderNames.FoundationaLLM_Agent}/_agent-references.json";

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Agent;

        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> _resourceTypes =>
            new()
            {
                {
                    AgentResourceTypeNames.Agents,
                    new ResourceTypeDescriptor(
                        AgentResourceTypeNames.Agents)
                    {
                        Actions = ["checkname"]
                    }
                },
                {
                    AgentResourceTypeNames.AgentReferences,
                    new ResourceTypeDescriptor(AgentResourceTypeNames.AgentReferences)
                }
            };

        /// <inheritdoc/>
        protected override async Task InitializeInternal()
        {
            _logger.LogInformation("Starting to initialize the {ResourceProvider} resource provider...", _name);

            if (await _storageService.FileExistsAsync(_storageContainerName, AGENT_REFERENCES_FILE_PATH, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, AGENT_REFERENCES_FILE_PATH, default);
                var agentReferenceStore = JsonConvert.DeserializeObject<AgentReferenceStore>(
                    Encoding.UTF8.GetString(fileContent.ToArray()));

                _agentReferences = new ConcurrentDictionary<string, AgentReference>(
                    agentReferenceStore!.ToDictionary());
            }
            else
            {
                await _storageService.WriteFileAsync(
                    _storageContainerName,
                    AGENT_REFERENCES_FILE_PATH,
                    JsonConvert.SerializeObject(new AgentReferenceStore { AgentReferences = [] }),
                    default,
                    default);
            }

            _logger.LogInformation("The {ResourceProvider} resource provider was successfully initialized.", _name);
        }

        /// <inheritdoc/>
        protected override async Task<string> GetResourcesAsyncInternal(List<ResourceTypeInstance> instances) =>
            instances[0].ResourceType switch
            {
                AgentResourceTypeNames.Agents => await LoadAndSerializeAgents(instances[0]),
                _ => throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.")
            };

        /// <inheritdoc/>
        protected override async Task UpsertResourceAsync(List<ResourceTypeInstance> instances, string serializedResource)
        {
            switch (instances[0].ResourceType)
            {
                case AgentResourceTypeNames.Agents:
                    await UpdateAgent(instances, serializedResource);
                    break;
                default:
                    throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.");
            }
        }

        private async Task<string> LoadAndSerializeAgents(ResourceTypeInstance instance)
        {
            if (instance.ResourceId == null)
            {
                var serializedAgents = new List<string>();

                foreach (var agentReference in _agentReferences.Values)
                {
                    var agent = await LoadAgent(agentReference);
                    serializedAgents.Add(
                        JsonConvert.SerializeObject(agent, agentReference.AgentType, _serializerSettings));
                }

                return $"[{string.Join(",", [.. serializedAgents])}]";
            }
            else
            {
                var agentReference = new AgentReference
                {
                    Name = instance.ResourceId,
                    Type = AgentTypes.KnowledgeManagement,
                    Filename = $"/{_name}/{instance.ResourceId}.json"
                };
                var agent = await LoadAgent(agentReference);

                if (!_agentReferences.ContainsKey(agentReference.Name))
                    _agentReferences[agentReference.Name] = agentReference;
                
                return JsonConvert.SerializeObject(agent, agentReference.AgentType, _serializerSettings);
            }
        }

        private async Task<AgentBase> LoadAgent(AgentReference agentReference)
        {
            if (await _storageService.FileExistsAsync(_storageContainerName, agentReference.Filename, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, agentReference.Filename, default);
                return JsonConvert.DeserializeObject(
                    Encoding.UTF8.GetString(fileContent.ToArray()),
                    agentReference.AgentType,
                    _serializerSettings) as AgentBase
                    ?? throw new ResourceProviderException($"Failed to load the agent {agentReference.Name}.");
            }

            throw new ResourceProviderException($"Could not locate the {agentReference.Name} agent resource.");
        }

        private async Task UpdateAgent(List<ResourceTypeInstance> instances, string serializedAgent)
        {
            var agentBase = JsonConvert.DeserializeObject<AgentBase>(serializedAgent)
                ?? throw new ResourceProviderException("The object definition is invalid.");

            if (instances[0].ResourceId != agentBase.Name)
                throw new ResourceProviderException("The resource path does not match the object definition (name mismatch).");

            var agentReference = new AgentReference
            {
                Name = agentBase.Name!,
                Type = agentBase.Type!,
                Filename = $"/{_name}/{agentBase.Name}.json"
            };

            var agent = JsonConvert.DeserializeObject(serializedAgent, agentReference.AgentType, _serializerSettings);
            (agent as AgentBase)!.ObjectId = GetObjectId(instances);

            await _storageService.WriteFileAsync(
                _storageContainerName,
                agentReference.Filename,
                JsonConvert.SerializeObject(agent, agentReference.AgentType, _serializerSettings),
                default,
                default);

            _agentReferences.AddOrUpdate(agentReference.Name, agentReference, (k,v) => agentReference);

            await _storageService.WriteFileAsync(
                    _storageContainerName,
                    AGENT_REFERENCES_FILE_PATH,
                    JsonConvert.SerializeObject(AgentReferenceStore.FromDictionary(_agentReferences.ToDictionary())),
                    default,
                    default);
        }

        #region Event handling

        /// <inheritdoc/>
        protected override async Task HandleEvents(EventSetEventArgs e)
        {
            _logger.LogInformation("{EventsCount} events received in the {EventsNamespace} events namespace.",
                e.Events.Count, e.Namespace);

            switch (e.Namespace)
            {
                case EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Agent:
                    foreach (var @event in e.Events)
                        await HandleAgentResourceProviderEvent(@event);
                    break;
                default:
                    // Ignore sliently any event namespace that's of no interest.
                    break;
            }

            await Task.CompletedTask;
        }

        private async Task HandleAgentResourceProviderEvent(CloudEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.Subject))
                return;

            var fileName = e.Subject.Split("/").Last();

            _logger.LogInformation("The file [{FileName}] managed by the [{ResourceProvider}] resource provider has changed and will be reloaded.",
                fileName, _name);

            var agentReference = new AgentReference
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Filename = $"/{_name}/{fileName}",
                Type = AgentTypes.Basic
            };

            var agent = await LoadAgent(agentReference);
            agentReference.Name = agent.Name;
            agentReference.Type = agent.Type;

            _agentReferences.AddOrUpdate(
                agentReference.Name,
                agentReference,
                (k, v) => v);

            _logger.LogInformation("The agent reference for the [{AgentName}] agent or type [{AgentType}] was loaded.",
                agentReference.Name, agentReference.Type);
        }

        #endregion
    }
}
