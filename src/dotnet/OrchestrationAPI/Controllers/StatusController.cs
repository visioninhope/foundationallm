using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Orchestration.API.Controllers
{
    /// <summary>
    /// Provides methods for checking the status of the service.
    /// </summary>
    /// <param name="orchestrationService">The <see cref="IOrchestrationService"/> that provides orchestration capabilities.</param>
    [ApiController]
    [Route("[controller]")]
    public class StatusController(
        IOrchestrationService orchestrationService) : ControllerBase
    {
        private readonly IOrchestrationService _orchestrationService = orchestrationService;

        /// <summary>
        /// Returns the status of the Orchestration API service.
        /// </summary>
        [HttpGet(Name = "GetServiceStatus")]
        public IActionResult Get() =>
            new OkObjectResult(new ServiceStatusInfo
            {
                Name = ServiceNames.OrchestrationAPI,
                Instance = ValidatedEnvironment.MachineName,
                Version = Environment.GetEnvironmentVariable(EnvironmentVariables.FoundationaLLM_Version),
                Status = _orchestrationService.Status
            });

        /// <summary>
        /// Returns the allowed HTTP methods for the Orchestration API service.
        /// </summary>
        [HttpOptions]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.Append("Allow", new[] { "GET", "POST", "OPTIONS" });

            return Ok();
        }
    }
}
