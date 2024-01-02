using Asp.Versioning;
using FoundationaLLM.Management.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Management.API.Controllers
{
    /// <summary>
    /// Provides methods for interacting with the Configuration Management service.
    /// </summary>
    [Authorize]
    [Authorize(Policy = "RequiredScope")]
    [ApiVersion(1.0)]
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigurationManagementService _configurationManagementService;
        private readonly ILogger<ConfigController> _logger;

        /// <summary>
        /// Constructor for the Config Controller.
        /// </summary>
        /// <param name="configurationManagementService">The Configuration Management service
        /// provides methods for managing configurations for FoundationaLLM.</param>
        /// <param name="logger">The logging interface used to log under the
        /// <see cref="ConfigController"/> type name.</param>
        public ConfigController(IConfigurationManagementService configurationManagementService,
            ILogger<ConfigController> logger)
        {
            _configurationManagementService = configurationManagementService;
            _logger = logger;
        }
    }
}
