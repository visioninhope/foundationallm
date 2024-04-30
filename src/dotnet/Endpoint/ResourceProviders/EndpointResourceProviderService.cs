using Azure.Messaging;
using FluentValidation;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Events;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;
using FoundationaLLM.Common.Services.ResourceProviders;
using FoundationaLLM.Endpoint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Endpoint.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Endpoint resource provider.
    /// </summary>
    /// <param name="instanceOptions">The options providing the <see cref="InstanceSettings"/> with instance settings.</param>
    /// <param name="authorizationService">The <see cref="IAuthorizationService"/> providing authorization services.</param>
    /// <param name="storageService">The <see cref="IStorageService"/> providing storage services.</param>
    /// <param name="eventService">The <see cref="IEventService"/> providing event services.</param>
    /// <param name="resourceValidatorFactory">The <see cref="IResourceValidatorFactory"/> providing the factory to create resource validators.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> of the main dependency injection container.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to provide loggers for logging.</param>
    public class EndpointResourceProviderService(
        IOptions<InstanceSettings> instanceOptions,
        IAuthorizationService authorizationService,
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Endpoint)] IStorageService storageService,
        IEventService eventService,
        IResourceValidatorFactory resourceValidatorFactory,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
        : ResourceProviderServiceBase(
            instanceOptions.Value,
            authorizationService,
            storageService,
            eventService,
            resourceValidatorFactory,
            serviceProvider,
            loggerFactory.CreateLogger<EndpointResourceProviderService>(),
            [
                EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Endpoint
            ])
    {
        private const string ENDPOINT_REFERENCES_FILE_NAME = "_endpoint-references.json";
        private const string ENDPOINT_REFERENCES_FILE_PATH = $"/{ResourceProviderNames.FoundationaLLM_Endpoint}/{ENDPOINT_REFERENCES_FILE_NAME}";

        private ConcurrentDictionary<string, EndpointReference> _endpointReferences = [];

        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> GetResourceTypes() =>
            EndpointResourceProviderMetadata.AllowedResourceTypes;

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Endpoint;

        /// <inheritdoc/>
        protected override async Task InitializeInternal()
        {
            _logger.LogInformation("Starting to initialize the {ResourceProvider} resource provider...", _name);

            if (await _storageService.FileExistsAsync(_storageContainerName, ENDPOINT_REFERENCES_FILE_PATH, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, ENDPOINT_REFERENCES_FILE_PATH, default);
                var endpointReferenceStore = JsonSerializer.Deserialize<EndpointReferenceStore>(
                    Encoding.UTF8.GetString(fileContent.ToArray()));

                _endpointReferences = new ConcurrentDictionary<string, EndpointReference>(
                    endpointReferenceStore!.ToDictionary());
            }
            else
            {
                await _storageService.WriteFileAsync(
                    _storageContainerName,
                    ENDPOINT_REFERENCES_FILE_PATH,
                    JsonSerializer.Serialize(new EndpointReferenceStore { EndpointReferences = [] }),
                    default,
                    default);
            }

            _logger.LogInformation("The {ResourceProvider} resource provider was successfully initialized.", _name);
        }

        #region Support for Management API

        /// <inheritdoc/>
        protected override async Task<object> GetResourcesAsync(ResourcePath resourcePath, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances[0].ResourceType switch
            {
                EndpointResourceTypeNames.Endpoints => await LoadEndpoints(resourcePath.ResourceTypeInstances[0]),
                _ => throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances[0].ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest)
            };

        #region Helpers for GetResourcesAsyncInternal

        private async Task<List<EndpointBase>> LoadEndpoints(ResourceTypeInstance instance)
        {
            if (instance.ResourceId == null)
            {
                return
                [
                    .. (await Task.WhenAll(
                        _endpointReferences.Values
                            .Where(er => !er.Deleted)
                            .Select(er => LoadEndpoint(er))))
                ];
            }
            else
            {
                if (!_endpointReferences.TryGetValue(instance.ResourceId, out var endpointReference)
                    || endpointReference.Deleted)
                    throw new ResourceProviderException($"Could not locate the {instance.ResourceId} endpoint resource.",
                        StatusCodes.Status404NotFound);

                var endpoint = await LoadEndpoint(endpointReference!);

                return [endpoint];
            }
        }

        private async Task<EndpointBase> LoadEndpoint(EndpointReference endpointReference)
        {
            if (await _storageService.FileExistsAsync(_storageContainerName, endpointReference.Filename, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, endpointReference.Filename, default);
                return JsonSerializer.Deserialize(
                    Encoding.UTF8.GetString(fileContent.ToArray()),
                    endpointReference.EndpointType,
                    _serializerSettings) as EndpointBase
                    ?? throw new ResourceProviderException($"Failed to load the endpoint {endpointReference.Name}.",
                        StatusCodes.Status400BadRequest);
            }
            else
                throw new ResourceProviderException($"Could not locate the {endpointReference.Name} endpoint resource.",
                    StatusCodes.Status404NotFound);
        }

        #endregion

        /// <inheritdoc/>
        protected override async Task<object> UpsertResourceAsync(ResourcePath resourcePath, string serializedResource, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances[0].ResourceType switch
            {
                EndpointResourceTypeNames.Endpoints => await UpdateEndpoint(resourcePath, serializedResource),
                _ => throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances[0].ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest)
            };

        #region Helpers for UpsertResourceAsync

        private async Task<ResourceProviderUpsertResult> UpdateEndpoint(ResourcePath resourcePath, string serializedEndpoint)
        {
            var endpoint = JsonSerializer.Deserialize<EndpointBase>(serializedEndpoint)
                ?? throw new ResourceProviderException("The object definition is invalid.",
                    StatusCodes.Status400BadRequest);

            if (_endpointReferences.TryGetValue(endpoint.Name!, out var existingEndpointReference)
                && existingEndpointReference!.Deleted)
                throw new ResourceProviderException($"The endpoint resource {existingEndpointReference.Name} cannot be added or updated.",
                        StatusCodes.Status400BadRequest);

            if (resourcePath.ResourceTypeInstances[0].ResourceId != endpoint.Name)
                throw new ResourceProviderException("The resource path does not match the object definition (name mismatch).",
                    StatusCodes.Status400BadRequest);

            var endpointReference = new EndpointReference
            {
                Name = endpoint.Name!,
                Type = endpoint.Type!,
                Filename = $"/{_name}/{endpoint.Name}.json",
                Deleted = false
            };

            endpoint.ObjectId = resourcePath.GetObjectId(_instanceSettings.Id, _name);

            var validator = _resourceValidatorFactory.GetValidator(endpointReference.EndpointType);
            if (validator is IValidator endpointValidator)
            {
                var context = new ValidationContext<object>(endpoint);
                var validationResult = await endpointValidator.ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    throw new ResourceProviderException($"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        StatusCodes.Status400BadRequest);
                }
            }

            await _storageService.WriteFileAsync(
                _storageContainerName,
                endpointReference.Filename,
                JsonSerializer.Serialize<EndpointBase>(endpoint, _serializerSettings),
                default,
                default);

            _endpointReferences.AddOrUpdate(endpointReference.Name, endpointReference, (k, v) => endpointReference);

            await _storageService.WriteFileAsync(
                    _storageContainerName,
                    ENDPOINT_REFERENCES_FILE_PATH,
                    JsonSerializer.Serialize(EndpointReferenceStore.FromDictionary(_endpointReferences.ToDictionary())),
                    default,
                    default);

            return new ResourceProviderUpsertResult
            {
                ObjectId = (endpoint as EndpointBase)!.ObjectId
            };
        }

        #endregion

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<object> ExecuteActionAsync(ResourcePath resourcePath, string serializedAction, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances.Last().ResourceType switch
            {
                EndpointResourceTypeNames.Endpoints => resourcePath.ResourceTypeInstances.Last().Action switch
                {
                    EndpointResourceProviderActions.CheckName => CheckResourceName(serializedAction),
                    _ => throw new ResourceProviderException($"The action {resourcePath.ResourceTypeInstances.Last().Action} is not supported by the {_name} resource provider.",
                        StatusCodes.Status400BadRequest)
                },
                _ => throw new ResourceProviderException()
            };
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        #region Helpers for ExecuteActionAsync

        private ResourceNameCheckResult CheckResourceName(string serializedAction)
        {
            var resourceName = JsonSerializer.Deserialize<ResourceName>(serializedAction);
            return _endpointReferences.Values.Any(ar => ar.Name.Equals(resourceName!.Name, StringComparison.OrdinalIgnoreCase))
                ? new ResourceNameCheckResult
                {
                    Name = resourceName!.Name,
                    Type = resourceName.Type,
                    Status = NameCheckResultType.Denied,
                    Message = "A resource with the specified name already exists or was previously deleted and not purged."
                }
                : new ResourceNameCheckResult
                {
                    Name = resourceName!.Name,
                    Type = resourceName.Type,
                    Status = NameCheckResultType.Allowed
                };
        }

        #endregion

        /// <inheritdoc/>
        protected override async Task DeleteResourceAsync(ResourcePath resourcePath, UnifiedUserIdentity userIdentity)
        {
            switch (resourcePath.ResourceTypeInstances.Last().ResourceType)
            {
                case EndpointResourceTypeNames.Endpoints:
                    await DeleteEndpoint(resourcePath.ResourceTypeInstances);
                    break;
                default:
                    throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances.Last().ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest);
            };
        }

        #region Helpers for DeleteResourceAsync

        private async Task DeleteEndpoint(List<ResourceTypeInstance> instances)
        {
            if (_endpointReferences.TryGetValue(instances.Last().ResourceId!, out var endpointReference))
            {
                if (!endpointReference.Deleted)
                {
                    endpointReference.Deleted = true;

                    await _storageService.WriteFileAsync(
                        _storageContainerName,
                        ENDPOINT_REFERENCES_FILE_PATH,
                        JsonSerializer.Serialize(EndpointReferenceStore.FromDictionary(_endpointReferences.ToDictionary())),
                        default,
                        default);
                }
            }
            else
            {
                throw new ResourceProviderException($"Could not locate the {instances.Last().ResourceId} endpoint resource.",
                    StatusCodes.Status404NotFound);
            }
        }

        #endregion

        #endregion

        /// <inheritdoc/>
        protected override T GetResourceInternal<T>(ResourcePath resourcePath) where T : class
        {
            if (resourcePath.ResourceTypeInstances.Count != 1)
                throw new ResourceProviderException($"Invalid resource path");

            if (typeof(T) != typeof(EndpointBase))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({resourcePath.ResourceTypeInstances[0].ResourceType}).");

            _endpointReferences.TryGetValue(resourcePath.ResourceTypeInstances[0].ResourceId!, out var endpointReference);
            if (endpointReference == null || endpointReference.Deleted)
                throw new ResourceProviderException($"The resource {resourcePath.ResourceTypeInstances[0].ResourceId!} of type {resourcePath.ResourceTypeInstances[0].ResourceType} was not found.");

            var endpoint = LoadEndpoint(endpointReference).Result;
            return endpoint as T
                ?? throw new ResourceProviderException($"The resource {resourcePath.ResourceTypeInstances[0].ResourceId!} of type {resourcePath.ResourceTypeInstances[0].ResourceType} was not found.");
        }

        #region Event handling

        /// <inheritdoc/>
        protected override async Task HandleEvents(EventSetEventArgs e)
        {
            _logger.LogInformation("{EventsCount} events received in the {EventsNamespace} events namespace.",
                e.Events.Count, e.Namespace);

            switch (e.Namespace)
            {
                case EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Endpoint:
                    foreach (var @event in e.Events)
                        await HandleEndpointResourceProviderEvent(@event);
                    break;
                default:
                    // Ignore sliently any event namespace that's of no interest.
                    break;
            }

            await Task.CompletedTask;
        }

        private async Task HandleEndpointResourceProviderEvent(CloudEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.Subject))
                return;

            var fileName = e.Subject.Split("/").Last();

            _logger.LogInformation("The file [{FileName}] managed by the [{ResourceProvider}] resource provider has changed and will be reloaded.",
                fileName, _name);

            var endpointReference = new EndpointReference
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Filename = $"/{_name}/{fileName}",
                Type = EndpointTypes.Basic,
                Deleted = false
            };

            var endpoint = await LoadEndpoint(endpointReference);
            endpointReference.Name = endpoint.Name;
            endpointReference.Type = endpoint.Type!;

            _endpointReferences.AddOrUpdate(
                endpointReference.Name,
                endpointReference,
                (k, v) => v);

            _logger.LogInformation("The endpoint reference for the [{EndpointName}] endpoint or type [{EndpointType}] was loaded.",
                endpointReference.Name, endpointReference.Type);
        }

        #endregion
    }
}
