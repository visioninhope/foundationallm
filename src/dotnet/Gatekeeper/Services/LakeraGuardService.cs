using Azure.AI.ContentSafety;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gatekeeper.Core.Interfaces;
using FoundationaLLM.Gatekeeper.Core.Models.ConfigurationOptions;
using FoundationaLLM.Gatekeeper.Core.Models.ContentSafety;
using FoundationaLLM.Gatekeeper.Core.Models.LakeraGuard;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FoundationaLLM.Gatekeeper.Core.Services
{
    /// <summary>
    /// Implements the <see cref="ILakeraGuardService"/> interface.
    /// </summary>
    public class LakeraGuardService : ILakeraGuardService
    {
        private readonly HttpClient _client;
        private readonly LakeraGuardServiceSettings _settings;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for the Azure Content Safety service.
        /// </summary>
        /// <param name="options">The configuration options for the Azure Content Safety service.</param>
        /// <param name="logger">The logger for the Azure Content Safety service.</param>
        public LakeraGuardService(
            IOptions<LakeraGuardServiceSettings> options,
            ILogger<LakeraGuardServiceSettings> logger)
        {
            _settings = options.Value;
            _logger = logger;

            _client = new HttpClient();

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.APIKey}");
            _client.BaseAddress = new Uri(_settings.APIUrl);
        }

        /// <inheritdoc/>
        public async Task<string?> DetectPromptInjection(string content)
        {
            var response = await _client.PostAsync("prompt_injection",
                new StringContent(JsonSerializer.Serialize(new { input = content }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<AnalyzePromptInjectionResult>(responseContent);
                var promptinjectionResult = results!.Results.FirstOrDefault();

                if (promptinjectionResult != null && promptinjectionResult.Flagged)
                {
                    if (promptinjectionResult.Categories["prompt_injection"])
                        return "The prompt text did not pass the safety filter. Reason: Prompt injection detected.";

                    if (promptinjectionResult.Categories["jailbreak"])
                        return "The prompt text did not pass the safety filter. Reason: Prompt jailbreak detected.";
                }
            }

            return null;
        }
    }
}
