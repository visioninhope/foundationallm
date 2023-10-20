using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Core.Services
{
    public class GatekeeperAPIService : IGatekeeperAPIService
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly ILogger<GatekeeperAPIService> _logger;

        public GatekeeperAPIService(
            IHttpClientFactoryService httpClientFactoryService, 
            IUserIdentityContext userIdentityContext,
            ILogger<GatekeeperAPIService> logger
            )
        {
            _httpClientFactoryService = httpClientFactoryService;
            _jsonSerializerSettings = CommonJsonSerializerSettings.GetJsonSerializerSettings();
            _logger = logger;
        }

        public async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            // TODO: Call RefinementService to refine userPrompt
            // await _refinementService.RefineUserPrompt(completionRequest);

            using var activity = Common.Logging.ActivitySources.CoreAPIActivitySource.StartActivity("GetCompletion");

            var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.GatekeeperAPI, string.Empty);

            var responseMessage = await client.PostAsync("orchestration/completion",
            new StringContent(
                    JsonConvert.SerializeObject(completionRequest, _jsonSerializerSettings),
                    Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse = JsonConvert.DeserializeObject<CompletionResponse>(responseContent);

                return completionResponse;
            }

            return new CompletionResponse
            {
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = completionRequest.UserPrompt,
                PromptTokens = 0,
                CompletionTokens = 0,
                UserPromptEmbedding = new float[] { 0 }
            };
        }

        public async Task<string> GetSummary(string content)
        {
            // TODO: Call RefinementService to refine userPrompt
            // await _refinementService.RefineUserPrompt(content);

            using var activity = Common.Logging.ActivitySources.CoreAPIActivitySource.StartActivity("GetSummary");

            var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.GatekeeperAPI, string.Empty);

            var responseMessage = await client.PostAsync("orchestration/summary",
                new StringContent(
                    JsonConvert.SerializeObject(new SummaryRequest { UserPrompt = content }, _jsonSerializerSettings),
                    Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var summarizeResponse = JsonConvert.DeserializeObject<SummaryResponse>(responseContent);

                return summarizeResponse?.Summary;
            }
            else
                return "A problem on my side prevented me from responding.";
        }

        public async Task<bool> SetLLMOrchestrationPreference(string orchestrationService)
        {
            var activity = Common.Logging.ActivitySources.CoreAPIActivitySource.CreateActivity("SetLLMOrchestrationPreference", System.Diagnostics.ActivityKind.Client);
            activity.Start();

            var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.GatekeeperAPI, activity.Id);

            var responseMessage = await client.PostAsync("orchestration/preference",
                new StringContent(orchestrationService));

            if (responseMessage.IsSuccessStatusCode)
            {
                // The response value should be a boolean indicating whether the orchestration service was set successfully.
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var orchestrationServiceSet = JsonConvert.DeserializeObject<bool>(responseContent);
                return orchestrationServiceSet;
            }
            
            return false;
        }

        public Task AddMemory(object item, string itemName, Action<object, float[]> vectorizer)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMemory(object item)
        {
            throw new NotImplementedException();
        }
    }
}
