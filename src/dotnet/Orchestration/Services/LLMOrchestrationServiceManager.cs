using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Orchestration.Core.Services
{
    /// <summary>
    /// Manages internal and external orchestration services.
    /// </summary>
    public class LLMOrchestrationServiceManager : ILLMOrchestrationServiceManager
    {
        private readonly Dictionary<string, IResourceProviderService> _resourceProviderServices;
        private readonly Dictionary<string, ILLMOrchestrationService> _orchestrationServices;
        private readonly ILogger<LLMOrchestrationServiceManager> _logger;

        private bool _initialized = false;

        /// <summary>
        /// Creates a new instance of the LLM Orchestration Service Manager.
        /// </summary>
        /// <param name="resourceProviderServices">A list of <see cref="IResourceProviderService"/> resource providers.</param>
        /// <param name="orchestrationServices">A list of <see cref="ILLMOrchestrationService"/> LLM orchestration services.</param>
        /// <param name="logger">The logger for the orchestration service manager.</param>
        public LLMOrchestrationServiceManager(
            IEnumerable<IResourceProviderService> resourceProviderServices,
            IEnumerable<ILLMOrchestrationService> orchestrationServices,
            ILogger<LLMOrchestrationServiceManager> logger)
        {
            _resourceProviderServices =
                resourceProviderServices.ToDictionary<IResourceProviderService, string>(rps => rps.Name);
            _orchestrationServices =
                orchestrationServices.ToDictionary<ILLMOrchestrationService, string>(os => os.GetType().Name);
            _logger = logger;

            // Kicks off the initialization on a separate thread and does not wait for it to complete.
            // The completion of the initialization process will be signaled by setting the _initialized property.
            _ = Task.Run(Initialize);
        }

        #region Initialization

        /// <summary>
        /// Performs the initialization of the orchestration service.
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            try
            {
                _logger.LogInformation("Starting to initialize the LLM Orchestration Service Manager service...");

                var configurationResourceProvider = _resourceProviderServices[ResourceProviderNames.FoundationaLLM_Configuration];

                var externalOrchestrationServices = await configurationResourceProvider.GetResources<ExternalOrchestrationService>(
                    DefaultAuthentication.ServiceIdentity!);

                  
                //configurationResourceProvider.GetResource<AppConfigurationKeyValue>


                _initialized = true;

                _logger.LogInformation("The LLM Orchestration Service Manager service was successfully initialized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing the LLM Orchestration Service Manager service.");
            }
        }

        #endregion

        /// <inheritdoc/>
        public string Status
        {
            get
            {
                var aggregateStatus = _orchestrationServices
                    .Select(x => new
                    {
                        Name = x.Key,
                        Initialized = x.Value.IsInitialized
                    })
                    .OrderBy(x => x.Name)
                    .ToList();


                if (aggregateStatus.All(s => s.Initialized))
                    return "ready";

                return string.Join(",", aggregateStatus
                    .Where(s => !s.Initialized)
                    .Select(s => $"{s.Name}: initializing"));
            }
        }

        /// <inheritdoc/>
        public ILLMOrchestrationService GetService(string serviceName)
        {
            if (!_orchestrationServices.TryGetValue(serviceName, out var service))
                throw new OrchestrationException($"The LLM orchestration service {serviceName} is not available.");

            return service;
        }
    }
}
