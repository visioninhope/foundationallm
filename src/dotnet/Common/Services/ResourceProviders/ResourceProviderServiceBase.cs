using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authorization;
using FoundationaLLM.Common.Models.Configuration.Events;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Events;
using FoundationaLLM.Common.Models.ResourceProvider;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Services.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Text.Json;

namespace FoundationaLLM.Common.Services.ResourceProviders
{
    /// <summary>
    /// Implements basic resource provider functionality
    /// </summary>
    public class ResourceProviderServiceBase : IResourceProviderService
    {
        private bool _isInitialized = false;

        private LocalEventService? _localEventService;
        private readonly List<string>? _eventNamespacesToSubscribe;
        private readonly ImmutableList<string> _allowedResourceProviders;
        private readonly Dictionary<string, ResourceTypeDescriptor> _allowedResourceTypes;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The <see cref="IAuthorizationService"/> providing authorization services to the resource provider.
        /// </summary>
        protected readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// The <see cref="IStorageService"/> providing storage services to the resource provider.
        /// </summary>
        protected readonly IStorageService _storageService;

        /// <summary>
        /// The <see cref="IEventService"/> providing event services to the resource provider.
        /// </summary>
        protected readonly IEventService _eventService;

        /// <summary>
        /// The <see cref="IResourceValidatorFactory"/> providing services to instantiate resource validators.
        /// </summary>
        protected readonly IResourceValidatorFactory _resourceValidatorFactory;

        /// <summary>
        /// The logger used for logging.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// The <see cref="InstanceSettings"/> that provides instance-wide settings.
        /// </summary>
        protected readonly InstanceSettings _instanceSettings;

        /// <summary>
        /// The name of the storage container name used by the resource provider to store its internal data.
        /// </summary>
        protected virtual string _storageContainerName => "resource-provider";

        /// <summary>
        /// The name of the resource provider. Must be overridden in derived classes.
        /// </summary>
        protected virtual string _name => throw new NotImplementedException();

        /// <summary>
        /// Default JSON serialization settings.
        /// </summary>
        protected virtual JsonSerializerOptions _serializerSettings => new()
        {
            WriteIndented = true
        };

        /// <inheritdoc/>
        public string Name => _name;

        /// <inheritdoc/>
        public bool IsInitialized  => _isInitialized;

        /// <summary>
        /// Creates a new instance of the resource provider.
        /// </summary>
        /// <param name="instanceSettings">The <see cref="InstanceSettings"/> that provides instance-wide settings.</param>
        /// <param name="authorizationService">The <see cref="IAuthorizationService"/> providing authorization services to the resource provider.</param>
        /// <param name="storageService">The <see cref="IStorageService"/> providing storage services to the resource provider.</param>
        /// <param name="eventService">The <see cref="IEventService"/> providing event services to the resource provider.</param>
        /// <param name="resourceValidatorFactory">The <see cref="IResourceValidatorFactory"/> providing services to instantiate resource validators.</param>
        /// <param name="logger">The logger used for logging.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> dependency injection service provider used to resolve scoped dependencies.</param>
        /// <param name="eventNamespacesToSubscribe">The list of Event Service event namespaces to subscribe to for local event processing.</param>
        public ResourceProviderServiceBase(
            InstanceSettings instanceSettings,
            IAuthorizationService authorizationService,
            IStorageService storageService,
            IEventService eventService,
            IResourceValidatorFactory resourceValidatorFactory,
            ILogger logger,
            IServiceProvider serviceProvider,
            List<string>? eventNamespacesToSubscribe = default)
        {
            _authorizationService = authorizationService;
            _storageService = storageService;
            _eventService = eventService;
            _resourceValidatorFactory = resourceValidatorFactory;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _instanceSettings = instanceSettings;
            _eventNamespacesToSubscribe = eventNamespacesToSubscribe;

            _allowedResourceProviders = [_name];
            _allowedResourceTypes = GetResourceTypes();

            // Kicks off the initialization on a separate thread and does not wait for it to complete.
            // The completion of the initialization process will be signaled by setting the _isInitialized property.
            _ = Task.Run(Initialize);
        }

        /// <inheritdoc/>
        private async Task Initialize()
        {
            try
            {
                await InitializeInternal();

                if (_eventNamespacesToSubscribe != null
                    && _eventNamespacesToSubscribe.Count > 0)
                {
                    _localEventService = new LocalEventService(
                        new LocalEventServiceSettings { EventProcessingCycleSeconds = 10 },
                        _eventService,
                        _logger);
                    _localEventService.SubscribeToEventNamespaces(_eventNamespacesToSubscribe);
                    _localEventService.StartLocalEventProcessing(HandleEvents);
                }

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The resource provider {ResourceProviderName} failed to initialize.", _name);
            }
        }

        #region IManagementProviderService

        /// <inheritdoc/>
        public async Task<object> HandleGetAsync(string resourcePath)
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(
                resourcePath,
                _allowedResourceProviders,
                _allowedResourceTypes,
                allowAction: false);

            // Authorize access to the resource path.
            await Authorize(parsedResourcePath, "read");

            return await GetResourcesAsyncInternal(parsedResourcePath);
        }

        /// <inheritdoc/>
        public async Task<object> HandlePostAsync(string resourcePath, string serializedResource)
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(
                resourcePath,
                _allowedResourceProviders,
                _allowedResourceTypes);

            // Authorize access to the resource path.
            await Authorize(parsedResourcePath, "write");

            if (parsedResourcePath.ResourceTypeInstances.Last().Action != null)
                return await ExecuteActionAsync(parsedResourcePath, serializedResource);
            else
                return await UpsertResourceAsync(parsedResourcePath, serializedResource);
        }

        /// <inheritdoc/>
        public async Task HandleDeleteAsync(string resourcePath)
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(
                resourcePath,
                _allowedResourceProviders,
                _allowedResourceTypes,
                allowAction: false);

            // Authorize access to the resource path.
            await Authorize(parsedResourcePath, "delete");

            await DeleteResourceAsync(parsedResourcePath);
        }

        #region Virtuals to be overriden in derived classes

        /// <summary>
        /// The internal implementation of GetResourcesAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task<object> GetResourcesAsyncInternal(ResourcePath resourcePath)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of UpsertResourceAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <param name="serializedResource">The serialized resource being created or updated.</param>
        /// <returns></returns>
        protected virtual async Task<object> UpsertResourceAsync(ResourcePath resourcePath, string serializedResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of ExecuteActionAsync. Must be overriden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <param name="serializedAction">The serialized details of the action being executed.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual async Task<object> ExecuteActionAsync(ResourcePath resourcePath, string serializedAction)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of DeleteResourceAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task DeleteResourceAsync(ResourcePath resourcePath)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region IResourceProviderService

        /// <inheritdoc/>
        public IList<T> GetResources<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            return GetResourcesInternal<T>(parsedResourcePath);
        }

        /// <inheritdoc/>
        public async Task<IList<T>> GetResourcesAsync<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            return await GetResourcesAsyncInternal<T>(parsedResourcePath);
        }

        /// <inheritdoc/>
        public T GetResource<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            return GetResourceInternal<T>(parsedResourcePath);
        }

        /// <inheritdoc/>
        public async Task<T> GetResourceAsync<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            return await GetResourceAsyncInternal<T>(parsedResourcePath);
        }

        /// <inheritdoc/>
        public async Task<string> UpsertResourceAsync<T>(string resourcePath, T resource) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            await UpsertResourceAsync<T>(parsedResourcePath, resource);
            return parsedResourcePath.GetObjectId(_instanceSettings.Id, _name);
        }

        /// <inheritdoc/>
        public string UpsertResource<T>(string resourcePath, T resource) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            UpsertResource<T>(parsedResourcePath, resource);
            return parsedResourcePath.GetObjectId(_instanceSettings.Id, _name);
        }    

        /// <inheritdoc/>
        public async Task DeleteResourceAsync<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            await DeleteResourceAsync<T>(parsedResourcePath);
        }

        /// <inheritdoc/>
        public void DeleteResource<T>(string resourcePath) where T : class
        {
            if (!_isInitialized)
                throw new ResourceProviderException($"The resource provider {_name} is not initialized.");
            var parsedResourcePath = new ResourcePath(resourcePath, _allowedResourceProviders, _allowedResourceTypes);
            DeleteResource<T>(parsedResourcePath);
        }

        #region Virtuals to be overriden in derived classes

        /// <summary>
        /// The internal implementation of Initialize. Must be overridden in derived classes.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task InitializeInternal()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the resource types dictionary. Must be overriden in derived classes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual Dictionary<string, ResourceTypeDescriptor> GetResourceTypes() =>
            throw new NotImplementedException();

        /// <summary>
        /// The internal implementation of ExecuteAction. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task<ResourceProviderActionResult> ExecuteActionInternal(ResourcePath resourcePath)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of GetResources. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual IList<T> GetResourcesInternal<T>(ResourcePath resourcePath) where T : class =>
            throw new NotImplementedException();

        /// <summary>
        /// The internal implementation of GetResourcesAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task<IList<T>> GetResourcesAsyncInternal<T>(ResourcePath resourcePath) where T : class
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of GetResource. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual T GetResourceInternal<T>(ResourcePath resourcePath) where T : class =>
            throw new NotImplementedException();

        /// <summary>
        /// The internal implementation of GetResourceAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task<T> GetResourceAsyncInternal<T>(ResourcePath resourcePath) where T : class
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of UpsertResource. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <param name="resource">The instance of the resource being created or updated.</param>
        /// <returns></returns>
        protected virtual void UpsertResource<T>(ResourcePath resourcePath, T resource) =>
            throw new NotImplementedException();

        /// <summary>
        /// The internal implementation of UpsertResourceAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <param name="resource">The instance of the resource being created or updated.</param>
        /// <returns></returns>
        protected virtual async Task UpsertResourceAsync<T>(ResourcePath resourcePath, T resource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The internal implementation of DeleteResource. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual void DeleteResource<T>(ResourcePath resourcePath) =>
            throw new NotImplementedException();

        /// <summary>
        /// The internal implementation of DeleteResourceAsync. Must be overridden in derived classes.
        /// </summary>
        /// <param name="resourcePath">A <see cref="ResourcePath"/> containing information about the resource path.</param>
        /// <returns></returns>
        protected virtual async Task DeleteResourceAsync<T>(ResourcePath resourcePath)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Authorization

        /// <summary>
        /// Authorizes the specified action on a resource path.
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        /// <exception cref="ResourceProviderException"></exception>
        private async Task Authorize(ResourcePath resourcePath, string actionType)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var callContext = scope.ServiceProvider.GetRequiredService<ICallContext>();

                if (callContext.CurrentUserIdentity == null
                    || callContext.CurrentUserIdentity.UserId == null)
                    throw new Exception("The call context does not contain enough information for authorizaiton.");

                var result = await _authorizationService.ProcessAuthorizationRequest(new ActionAuthorizationRequest
                {
                    Action = $"{_name}/{resourcePath.MainResourceType}/{actionType}",
                    ResourcePath = resourcePath.GetObjectId(_instanceSettings.Id, _name),
                    PrincipalId = callContext.CurrentUserIdentity.UserId,
                    SecurityGroupIds = callContext.CurrentUserIdentity.GroupIds
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to authorize access to the resource path.");
                throw new ResourceProviderException(
                    "An error occurred while attempting to authorize access to the resource path.",
                    StatusCodes.Status403Forbidden);
            }
        }

        #endregion

        #region Events handling

        /// <summary>
        /// Handles events received from the <see cref="IEventService"/> when they are dequeued locally.
        /// </summary>
        /// <param name="e">The <see cref="EventSetEventArgs"/> containing the events namespace and the actual events.</param>
        /// <returns></returns>
        protected virtual async Task HandleEvents(EventSetEventArgs e) =>
            await Task.CompletedTask;

        #endregion
    }
}
