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
using FoundationaLLM.Common.Models.ResourceProviders.Model;
using FoundationaLLM.Common.Services.ResourceProviders;
using FoundationaLLM.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Model.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Model resource provider.
    /// </summary>
    /// <param name="instanceOptions">The options providing the <see cref="InstanceSettings"/> with instance settings.</param>
    /// <param name="authorizationService">The <see cref="IAuthorizationService"/> providing authorization services.</param>
    /// <param name="storageService">The <see cref="IStorageService"/> providing storage services.</param>
    /// <param name="eventService">The <see cref="IEventService"/> providing event services.</param>
    /// <param name="resourceValidatorFactory">The <see cref="IResourceValidatorFactory"/> providing the factory to create resource validators.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> of the main dependency injection container.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to provide loggers for logging.</param>
    public class ModelResourceProviderService(
        IOptions<InstanceSettings> instanceOptions,
        IAuthorizationService authorizationService,
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Model)]IStorageService storageService,
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
            loggerFactory.CreateLogger<ModelResourceProviderService>(),
            [
                EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Model
            ])
    {
        private const string MODEL_REFERENCES_FILE_NAME = "_model-references.json";
        private const string MODEL_REFERENCES_FILE_PATH = $"{ResourceProviderNames.FoundationaLLM_Model}/{MODEL_REFERENCES_FILE_NAME}";

        private ConcurrentDictionary<string, ModelReference> _modelReferences = [];

        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> GetResourceTypes() =>
            ModelResourceProviderMetadata.AllowedResourceTypes;

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Model;

        /// <inheritdoc/>
        protected override async Task InitializeInternal()
        {
            _logger.LogInformation("Starting to initialize the {ResourceProvider} resource provider...", _name);

            if (await _storageService.FileExistsAsync(_storageContainerName, MODEL_REFERENCES_FILE_PATH, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, MODEL_REFERENCES_FILE_PATH, default);
                var modelReferenceStore = JsonSerializer.Deserialize<ModelReferenceStore>(
                    Encoding.UTF8.GetString(fileContent.ToArray()));

                _modelReferences = new ConcurrentDictionary<string, ModelReference>(modelReferenceStore!.ToDictionary());
            }
            else
            {
                await _storageService.WriteFileAsync(
                    _storageContainerName,
                    MODEL_REFERENCES_FILE_PATH,
                    JsonSerializer.Serialize(new ModelReferenceStore { ModelReferences = [] }),
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
                ModelResourceTypeNames.Models => await LoadModels(resourcePath.ResourceTypeInstances[0]),
                _ => throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances[0].ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status404NotFound)
            };

        #region Helpers for GetResourcesAsyncInternal

        private async Task<List<ModelBase>> LoadModels(ResourceTypeInstance instance)
        {
            if (instance.ResourceId == null)
            {
                return
                [
                    .. (await Task.WhenAll(
                        _modelReferences.Values
                            .Where(mr => !mr.Deleted)
                            .Select(mr => LoadModel(mr))))
                ];
            }
            else
            {
                if (!_modelReferences.TryGetValue(instance.ResourceId, out var modelReference)
                    || modelReference.Deleted)
                    throw new ResourceProviderException($"Could not locate the {instance.ResourceId} model resource.",
                        StatusCodes.Status404NotFound);

                var model = await LoadModel(modelReference!);

                return [model];
            }
        }

        private async Task<ModelBase> LoadModel(ModelReference modelReference)
        {
            if (await _storageService.FileExistsAsync(_storageContainerName, modelReference.Filename, default))
            {
                var fileContent = await _storageService.ReadFileAsync(_storageContainerName, modelReference.Filename, default);
                return JsonSerializer.Deserialize(
                    Encoding.UTF8.GetString(fileContent.ToArray()),
                    modelReference.ModelType,
                    _serializerSettings) as ModelBase
                    ?? throw new ResourceProviderException($"Failed to load the model {modelReference.Name}.",
                                           StatusCodes.Status400BadRequest);
            }
            else
            {
                throw new ResourceProviderException($"Could not locate the {modelReference.Name} model resource.",
                        StatusCodes.Status404NotFound);
            }
        }

        #endregion

        /// <inheritdoc/>
        protected override async Task<object> UpsertResourceAsync(ResourcePath resourcePath, string serializedResource, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances[0].ResourceType switch
            {
                ModelResourceTypeNames.Models => await UpdateModel(resourcePath, serializedResource),
                _ => throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances[0].ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest)
            };

        #region Helpers for UpsertResourceAsync

        private async Task<ResourceProviderUpsertResult> UpdateModel(ResourcePath resourcePath, string serializedModel)
        {
            var model = JsonSerializer.Deserialize<ModelBase>(serializedModel)
                ?? throw new ResourceProviderException("The object definition is invalid.",
                    StatusCodes.Status400BadRequest);

            if (_modelReferences.TryGetValue(model.Name!, out var existingModelReference)
                && existingModelReference!.Deleted)
                throw new ResourceProviderException($"The model resource {existingModelReference.Name} cannot be added or updated.",
                        StatusCodes.Status400BadRequest);

            if (resourcePath.ResourceTypeInstances[0].ResourceId != model.Name)
                throw new ResourceProviderException("The resource path does not match the object definition (name mismatch).",
                    StatusCodes.Status400BadRequest);

            var modelReference = new ModelReference
            {
                Name = model.Name!,
                Type = model.Type!,
                Filename = $"/{_name}/{model.Name}.json",
                Deleted = false
            };

            model.ObjectId = resourcePath.GetObjectId(_instanceSettings.Id, _name);

            var validator = _resourceValidatorFactory.GetValidator(modelReference.ModelType);
            if (validator is IValidator modelValidator)
            {
                var context = new ValidationContext<object>(model);
                var validationResult = await modelValidator.ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    throw new ResourceProviderException($"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        StatusCodes.Status400BadRequest);
                }
            }

            await _storageService.WriteFileAsync(
                _storageContainerName,
                modelReference.Filename,
                JsonSerializer.Serialize<ModelBase>(model, _serializerSettings),
                default,
                default);

            _modelReferences.AddOrUpdate(modelReference.Name, modelReference, (k, v) => modelReference);

            await _storageService.WriteFileAsync(
                    _storageContainerName,
                    MODEL_REFERENCES_FILE_PATH,
                    JsonSerializer.Serialize(ModelReferenceStore.FromDictionary(_modelReferences.ToDictionary())),
                    default,
                    default);

            return new ResourceProviderUpsertResult
            {
                ObjectId = (model as ModelBase)!.ObjectId
            };
        }

        #endregion

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<object> ExecuteActionAsync(ResourcePath resourcePath, string serializedAction, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances.Last().ResourceType switch
            {
                ModelResourceTypeNames.Models => resourcePath.ResourceTypeInstances.Last().Action switch
                {
                    ModelResourceProviderActions.CheckName => CheckResourceName(serializedAction),
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
            return _modelReferences.Values.Any(mr => mr.Name.Equals(resourceName!.Name, StringComparison.OrdinalIgnoreCase))
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
                case ModelResourceTypeNames.Models:
                    await DeleteModel(resourcePath.ResourceTypeInstances);
                    break;
                default:
                    throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances.Last().ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest);
            };
        }

        #region Helpers for DeleteResourceAsync

        private async Task DeleteModel(List<ResourceTypeInstance> instances)
        {
            if (_modelReferences.TryGetValue(instances.Last().ResourceId!, out var modelReference))
            {
                if (!modelReference.Deleted)
                {
                    modelReference.Deleted = true;

                    await _storageService.WriteFileAsync(
                        _storageContainerName,
                        MODEL_REFERENCES_FILE_PATH,
                        JsonSerializer.Serialize(ModelReferenceStore.FromDictionary(_modelReferences.ToDictionary())),
                        default,
                        default);
                }
            }
            else
            {
                throw new ResourceProviderException($"Could not locate the {instances.Last().ResourceId} model resource.",
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

            if (typeof(T) != typeof(ModelBase))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({resourcePath.ResourceTypeInstances[0].ResourceType}).");

            _modelReferences.TryGetValue(resourcePath.ResourceTypeInstances[0].ResourceId!, out var modelReference);
            if (modelReference == null || modelReference.Deleted)
                throw new ResourceProviderException($"The resource {resourcePath.ResourceTypeInstances[0].ResourceId!} of type {resourcePath.ResourceTypeInstances[0].ResourceType} was not found.");

            var model = LoadModel(modelReference).Result;
            return model as T
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
                case EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Model:
                    foreach (var @event in e.Events)
                        await HandleModelResourceProviderEvent(@event);
                    break;
                default:
                    // Ignore sliently any event namespace that's of no interest.
                    break;
            }

            await Task.CompletedTask;
        }

        private async Task HandleModelResourceProviderEvent(CloudEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.Subject))
                return;

            var fileName = e.Subject.Split("/").Last();

            _logger.LogInformation("The file [{FileName}] managed by the [{ResourceProvider}] resource provider has changed and will be reloaded.",
                fileName, _name);

            var modelReference = new ModelReference
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Filename = $"/{_name}/{fileName}",
                Type = ModelTypes.Basic,
                Deleted = false
            };

            var model = await LoadModel(modelReference);
            modelReference.Name = model.Name;
            modelReference.Type = model.Type!;

            _modelReferences.AddOrUpdate(
                modelReference.Name,
                modelReference,
                (k, v) => v);

            _logger.LogInformation("The model reference for the [{ModelName}] model or type [{ModelType}] was loaded.",
                modelReference.Name, modelReference.Type);
        }

        #endregion
    }
}
