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
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Services.ResourceProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using FoundationaLLM.Common.Models.Chat;
using System.Linq;
using FoundationaLLM.Attachment.Models;

namespace FoundationaLLM.Attachment.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Attachment resource provider.
    /// </summary>
    /// <param name="instanceOptions">The options providing the <see cref="InstanceSettings"/> with instance settings.</param>
    /// <param name="authorizationService">The <see cref="IAuthorizationService"/> providing authorization services.</param>
    /// <param name="storageService">The <see cref="IStorageService"/> providing storage services.</param>
    /// <param name="eventService">The <see cref="IEventService"/> providing event services.</param>
    /// <param name="resourceValidatorFactory">The <see cref="IResourceValidatorFactory"/> providing the factory to create resource validators.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> of the main dependency injection container.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to provide loggers for logging.</param>
    public class AttachmentResourceProviderService(
        IOptions<InstanceSettings> instanceOptions,
        IAuthorizationService authorizationService,
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment)] IStorageService storageService,
        IEventService eventService,
        IResourceValidatorFactory resourceValidatorFactory,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
        : ResourceProviderServiceBase<AttachmentReference>(
            instanceOptions.Value,
            authorizationService,
            storageService,
            eventService,
            resourceValidatorFactory,
            serviceProvider,
            loggerFactory.CreateLogger<AttachmentResourceProviderService>(),
            [
                EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Attachment
            ],
            useInternalStore: true)
    {
        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> GetResourceTypes() =>
            AttachmentResourceProviderMetadata.AllowedResourceTypes;

        protected override string _name => ResourceProviderNames.FoundationaLLM_Attachment;

        protected override async Task InitializeInternal() =>
            await Task.CompletedTask;

        #region Resource provider support for Management API

        /// <inheritdoc/>
        protected override async Task<object> GetResourcesAsync(ResourcePath resourcePath, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances[0].ResourceType switch
            {
                AttachmentResourceTypeNames.Attachments => await LoadAttachments(resourcePath.ResourceTypeInstances[0], userIdentity),
                _ => throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances[0].ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest)
            };

        #region Helpers for GetResourcesAsyncInternal

        private async Task<List<ResourceProviderGetResult<AttachmentFile>>> LoadAttachments(ResourceTypeInstance instance, UnifiedUserIdentity userIdentity)
        {
            var attachments = new List<AttachmentFile>();

            if (instance.ResourceId == null)
            {
                var attachmentReferences = await _resourceReferenceStore!.GetAllResourceReferences();

                attachments = (await Task.WhenAll(attachmentReferences
                                         .Where(ar => !ar.Deleted)
                                         .Select(ar => LoadAttachment(ar))))
                                             .Where(a => a != null)
                                             .Select(a => a!)
                                             .ToList();
            }
            else
            {
                var attachmentReference = await _resourceReferenceStore!.GetResourceReference(instance.ResourceId);
                AttachmentFile? attachment;

                if (attachmentReference != null)
                {
                    attachment = await LoadAttachment(attachmentReference);
                    if (attachment != null)
                        attachments.Add(attachment);
                }
            }
            return attachments.Select(attachment => new ResourceProviderGetResult<AttachmentFile>() { Resource = attachment, Actions = [], Roles = [] }).ToList();
        }

        private async Task<AttachmentFile?> LoadAttachment(AttachmentReference attachmentReference, bool loadContent = false)
        {
            var attachmentFile = new AttachmentFile
            {
                ObjectId = attachmentReference.ObjectId,
                Name = attachmentReference.Name,
                OriginalFileName = attachmentReference.OriginalFilename,
                Type = attachmentReference.Type,
                Path = $"{_storageContainerName}{attachmentReference.Filename}",
                ContentType = attachmentReference.ContentType,
                SecondaryProvider = attachmentReference.SecondaryProvider
            };

            if (loadContent)
            {
                var fileContent = await _storageService.ReadFileAsync(
                    _storageContainerName,
                    attachmentReference.Filename,
                    default);
                attachmentFile.Content = fileContent.ToArray();
            }

            return attachmentFile;
        }

        #endregion

        /// <inheritdoc/>
        protected override Task<object> UpsertResourceAsync(ResourcePath resourcePath, string serializedResource, UnifiedUserIdentity userIdentity) => null;

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<object> ExecuteActionAsync(ResourcePath resourcePath, string serializedAction, UnifiedUserIdentity userIdentity) =>
            resourcePath.ResourceTypeInstances.Last().ResourceType switch
            {
                AttachmentResourceTypeNames.Attachments => resourcePath.ResourceTypeInstances.Last().Action switch
                {
                    ResourceProviderActions.Filter => await Filter(serializedAction),
                    _ => throw new ResourceProviderException($"The action {resourcePath.ResourceTypeInstances.Last().Action} is not supported by the {_name} resource provider.",
                        StatusCodes.Status400BadRequest)
                },
                _ => throw new ResourceProviderException()
            };
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        #region Helpers for ExecuteActionAsync

        private async Task<List<AttachmentDetail>> Filter(string serializedAction)
        {
            var resourceFilter = JsonSerializer.Deserialize<ResourceFilter>(serializedAction)
                ?? throw new ResourceProviderException("The object definition is invalid. Please provide a resource filter.",
                    StatusCodes.Status400BadRequest);
            if (resourceFilter.ObjectIDs is {Count: > 0})
            {
                var filteredReferences =
                    await _resourceReferenceStore!.GetResourceReferences(r => !string.IsNullOrWhiteSpace(r.ObjectId) && resourceFilter.ObjectIDs.Contains(r.ObjectId));

                return filteredReferences.Select(r => new AttachmentDetail
                {
                    ObjectId = r.ObjectId,
                    DisplayName = r.OriginalFilename,
                    ContentType = r.ContentType
                }).ToList();

            }

            var allResourceReferences = await _resourceReferenceStore!.GetAllResourceReferences();
            return allResourceReferences.Select(r => new AttachmentDetail
            {
                ObjectId = r.ObjectId,
                DisplayName = r.OriginalFilename,
                ContentType = r.ContentType
            }).ToList();
        }

        #endregion

        /// <inheritdoc/>
        protected override async Task DeleteResourceAsync(ResourcePath resourcePath, UnifiedUserIdentity userIdentity)
        {
            switch (resourcePath.ResourceTypeInstances.Last().ResourceType)
            {
                case AttachmentResourceTypeNames.Attachments:
                    await DeleteAttachment(resourcePath.ResourceTypeInstances);
                    break;
                default:
                    throw new ResourceProviderException($"The resource type {resourcePath.ResourceTypeInstances.Last().ResourceType} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest);
            };
        }

        #region Helpers for DeleteResourceAsync

        private async Task DeleteAttachment(List<ResourceTypeInstance> instances)
        {
            try
            {
                await _lock.WaitAsync();

                var attachmentReference = await _resourceReferenceStore!.GetResourceReference(instances.Last().ResourceId!);

                if (attachmentReference != null)
                {
                    if (!attachmentReference.Deleted)
                    {
                        attachmentReference.Deleted = true;

                        await _storageService.DeleteFileAsync(
                            _storageContainerName,
                            attachmentReference.Filename);

                        await _storageService.WriteFileAsync(
                            _storageContainerName,
                            ATTACHMENT_REFERENCES_FILE_PATH,
                            JsonSerializer.Serialize(new ResourceReferenceList<AttachmentReference> { ResourceReferences = _attachmentReferences.Values.ToList() }),
                            default,
                            default);
                    }
                }
                else
                {
                    throw new ResourceProviderException($"Could not locate the {instances.Last().ResourceId} attachment resource.",
                        StatusCodes.Status404NotFound);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        #endregion

        #endregion

        #region Resource provider strongly typed operations

        /// <inheritdoc/>
        protected override async Task<T> GetResourceInternal<T>(ResourcePath resourcePath, UnifiedUserIdentity userIdentity, ResourceProviderOptions? options = null) where T : class
        {
            _attachmentReferences.TryGetValue(resourcePath.ResourceTypeInstances[0].ResourceId!, out var attachmentReference);
            if (attachmentReference == null || attachmentReference.Deleted)
            {
                // Force a refresh of the references one time to make sure we don't have a stale copy.
                await InitializeInternal();
                _attachmentReferences.TryGetValue(resourcePath.ResourceTypeInstances[0].ResourceId!, out attachmentReference);

                if (attachmentReference == null || attachmentReference.Deleted)
                    throw new ResourceProviderException($"The resource {resourcePath.ResourceTypeInstances[0].ResourceId!} of type {resourcePath.ResourceTypeInstances[0].ResourceType} was not found.");
            }

            var attachment = await LoadAttachment(attachmentReference, loadContent: options?.LoadContent ?? false);
            return attachment as T
                ?? throw new ResourceProviderException($"The resource {resourcePath.ResourceTypeInstances[0].ResourceId!} of type {resourcePath.ResourceTypeInstances[0].ResourceType} was not found.");
        }

        /// <inheritdoc/>
        protected override async Task<TResult> UpsertResourceAsyncInternal<T, TResult>(ResourcePath resourcePath, T resource, UnifiedUserIdentity userIdentity) =>
            resource switch
            {
                AttachmentFile attachment => (TResult) await UpdateAttachment(resourcePath, attachment),
                _ => throw new ResourceProviderException(
                    $"The type {nameof(T)} is not supported by the {_name} resource provider.",
                    StatusCodes.Status400BadRequest)
            };

        #endregion

        #region Event handling

        /// <inheritdoc/>
        protected override async Task HandleEvents(EventSetEventArgs e)
        {
            _logger.LogInformation("{EventsCount} events received in the {EventsNamespace} events namespace.",
                e.Events.Count, e.Namespace);

            switch (e.Namespace)
            {
                case EventSetEventNamespaces.FoundationaLLM_ResourceProvider_Attachment:
                    foreach (var @event in e.Events)
                        await HandleAttachmentResourceProviderEvent(@event);
                    break;
                default:
                    // Ignore sliently any event namespace that's of no interest.
                    break;
            }

            await Task.CompletedTask;
        }

        private async Task HandleAttachmentResourceProviderEvent(CloudEvent e)
        {
            if (string.IsNullOrWhiteSpace(e.Subject))
                return;

            var fileName = e.Subject.Split("/").Last();

            _logger.LogInformation("The file [{FileName}] managed by the [{ResourceProvider}] resource provider has changed and will be reloaded.",
                fileName, _name);

            var attachmentReference = new AttachmentReference
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Filename = $"/{_name}/{fileName}",
                Type =nameof(AttachmentFile),
                Deleted = false
            };

            var attachment = await LoadAttachment(attachmentReference);
            attachmentReference.Name = attachment.Name;
            attachmentReference.Type = attachment.Type!;

            _attachmentReferences.AddOrUpdate(
                attachmentReference.Name,
                attachmentReference,
                (k, v) => v);

            _logger.LogInformation("The attachment reference for the [{AttachmentName}] agent or type [{AttachmentType}] was loaded.",
                attachmentReference.Name, attachmentReference.Type);
        }

        #endregion

        #region Resource management

        private async Task<ResourceProviderUpsertResult> UpdateAttachment(ResourcePath resourcePath, AttachmentFile attachment)
        {

            if (_attachmentReferences.TryGetValue(attachment.Name!, out var existingAttachmentReference)
                && existingAttachmentReference!.Deleted)
                throw new ResourceProviderException($"The attachment resource {existingAttachmentReference.Name} cannot be added or updated.",
                        StatusCodes.Status400BadRequest);

            if (resourcePath.ResourceTypeInstances[0].ResourceId != attachment.Name)
                throw new ResourceProviderException("The resource path does not match the object definition (name mismatch).",
                    StatusCodes.Status400BadRequest);

            var extension = GetFileExtension(attachment.DisplayName!);
            var fullName = $"{attachment.Name}{extension}";

            attachment.ObjectId = resourcePath.GetObjectId(_instanceSettings.Id, _name);
            var attachmentReference = new AttachmentReference
            {
                ObjectId = attachment.ObjectId,
                OriginalFilename = attachment.DisplayName!,
                ContentType = attachment.ContentType!,
                Name = attachment.Name,
                Type = nameof(AttachmentFile),
                Filename = $"/{_name}/{fullName}",
                SecondaryProvider = attachment.SecondaryProvider,
                Deleted = false
            };

            var validator = _resourceValidatorFactory.GetValidator(typeof(AttachmentFile));
            if (validator is IValidator attachmentValidator)
            {
                var context = new ValidationContext<object>(attachment);
                var validationResult = await attachmentValidator.ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    throw new ResourceProviderException($"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        StatusCodes.Status400BadRequest);
                }
            }

            await _storageService.WriteFileAsync(
                _storageContainerName,
                attachmentReference.Filename,
                new MemoryStream(attachment.Content!),
                attachment.ContentType ?? default,
                default);

            _attachmentReferences.AddOrUpdate(attachmentReference.Name, attachmentReference, (k, v) => attachmentReference);

            await _storageService.WriteFileAsync(
                    _storageContainerName,
                    ATTACHMENT_REFERENCES_FILE_PATH,
                    JsonSerializer.Serialize(new ResourceReferenceList<AttachmentReference> { ResourceReferences = _attachmentReferences.Values.ToList() }),
                    default,
                    default);

            return new ResourceProviderUpsertResult
            {
                ObjectId = (attachment as AttachmentFile)!.ObjectId
            };
        }

        private string GetFileExtension(string fileName) =>
            Path.GetExtension(fileName);

        #endregion
    }
}
