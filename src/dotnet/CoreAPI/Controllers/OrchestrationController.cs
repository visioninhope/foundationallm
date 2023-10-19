using Asp.Versioning;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Configuration.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace FoundationaLLM.Core.API.Controllers
{
    [Authorize]
    [Authorize(Policy = "RequiredScope")]
    [ApiVersion(1.0)]
    [ApiController]
    [Route("[controller]")]
    public class OrchestrationController : ControllerBase
    {
        private readonly IGatekeeperAPIService _gatekeeperAPIService;
        private readonly ILogger<OrchestrationController> _logger;

        private static readonly ActivitySource Activity = new(nameof(OrchestrationController));
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

        public OrchestrationController(IGatekeeperAPIService gatekeeperAPIService,
            ILogger<OrchestrationController> logger)
        {
            _gatekeeperAPIService = gatekeeperAPIService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("completion", Name = "GetCompletion")]
        public async Task<IActionResult> GetCompletion(CompletionRequest completionRequest)
        {
            using (var activity = Activity.StartActivity("GetCompletion", ActivityKind.Producer))
            {
                //var activity = Common.Logging.ActivitySources.CoreAPIActivitySource.CreateActivity("GetCompletion", System.Diagnostics.ActivityKind.Client);
                //Activity activity = new Activity("GetCompletion");

                if (this.Request.Headers.ContainsKey("correlationId"))
                {
                    string correlationId = this.Request.Headers["correlationId"];
                    activity.SetParentId(correlationId);
                    activity.SetTag("user_prompt", completionRequest.UserPrompt);
                    activity.Start();
                    completionRequest.CorrelationId = correlationId;
                }

                var completionResponse = await _gatekeeperAPIService.GetCompletion(completionRequest);

                if (this.Request.Headers.ContainsKey("correlationId"))
                    activity.Stop();

                return Ok(completionResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost("summary", Name = "GetSummary")]
        public async Task<IActionResult> GetSummary(SummaryRequest summaryRequest)
        {
            var summaryResponse = await _gatekeeperAPIService.GetSummary(summaryRequest.UserPrompt);

            return Ok(summaryResponse);
        }
    }
}
