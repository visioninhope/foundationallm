using FoundationaLLM.Authorization.Interfaces;
using FoundationaLLM.Authorization.Models;
using FoundationaLLM.Authorization.Models.Configuration;
using FoundationaLLM.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Authorization.Services
{
    /// <summary>
    /// Implements the core authorization engine.
    /// </summary>
    public class AuthorizationCore : IAuthorizationCore
    {
        private readonly IStorageService _storageService;
        private readonly IResourceValidatorFactory _resourceValidatorFactory;
        private readonly ILogger<AuthorizationCore> _logger;
        private readonly AuthorizationCoreSettings _settings;
        private readonly ConcurrentDictionary<string, RoleAssignmentStore> _roleAssignmentStores = [];

        private const string ROLE_ASSIGNMENTS_CONTAINER_NAME = "role-assignments";
        private bool _initialized = false;
        private object _syncRoot = new object();

        /// <summary>
        /// Creates a new instance of the <see cref="AuthorizationCore"/> class.
        /// </summary>
        /// <param name="storageService">The <see cref="IStorageService"/> providing storage services.</param>
        /// <param name="resourceValidatorFactory"> The resource validator factory used to create resource validators.</param>
        /// <param name="logger">The logger used for logging.</param>
        public AuthorizationCore(
            IOptions<AuthorizationCoreSettings> options,
            IStorageService storageService,
            IResourceValidatorFactory resourceValidatorFactory,
            ILogger<AuthorizationCore> logger)
        {
            _settings = options.Value;
            _storageService = storageService;
            _resourceValidatorFactory = resourceValidatorFactory;
            _logger = logger;

            // Kicks off the initialization on a separate thread and does not wait for it to complete.
            // The completion of the initialization process will be signaled by setting the _initialized property.
            _ = Task.Run(Initialize);
        }

        private async Task Initialize()
        {
            try
            {
                foreach (var instanceId in _settings.InstanceIds)
                {
                    var roleAssignmentStoreFile = $"/{instanceId.ToLower()}.json";
                    if (await _storageService.FileExistsAsync(ROLE_ASSIGNMENTS_CONTAINER_NAME, roleAssignmentStoreFile, default))
                    {
                        var fileContent = await _storageService.ReadFileAsync(ROLE_ASSIGNMENTS_CONTAINER_NAME, roleAssignmentStoreFile, default);
                        var roleAssignmentStore = JsonSerializer.Deserialize<RoleAssignmentStore>(
                            Encoding.UTF8.GetString(fileContent.ToArray()));
                        if (roleAssignmentStore == null
                            || string.Compare(roleAssignmentStore.InstanceId, instanceId) != 0)
                        {
                            _logger.LogError("The role assignment store file for instance {InstanceId} is invalid.", instanceId);
                        }
                        else
                        {
                            _roleAssignmentStores.AddOrUpdate(instanceId, roleAssignmentStore, (k, v) => roleAssignmentStore);
                            _logger.LogInformation("The role assignment store for instance {InstanceId} has been loaded.", instanceId);
                        }
                    }
                    else
                    {
                        var roleAssignmentStore = new RoleAssignmentStore
                        {
                            InstanceId = instanceId,
                            RoleAssignments = []
                        };

                        _roleAssignmentStores.AddOrUpdate(instanceId, roleAssignmentStore, (k, v) => roleAssignmentStore);
                        await _storageService.WriteFileAsync(
                            ROLE_ASSIGNMENTS_CONTAINER_NAME,
                            roleAssignmentStoreFile,
                            JsonSerializer.Serialize(roleAssignmentStore),
                            default,
                            default);
                        _logger.LogInformation("The role assignment store for instance {InstanceId} has been created.", instanceId);
                    }
                }

                _initialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The authorization core failed to initialize.");
            }
        }
    }
}
