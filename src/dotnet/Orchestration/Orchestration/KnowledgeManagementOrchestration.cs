using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Models.ResourceProviders.AzureOpenAI;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Orchestration.Core.Orchestration
{
    /// <summary>
    /// Knowledge Management orchestration.
    /// </summary>
    /// <remarks>
    /// Constructor for default agent.
    /// </remarks>
    /// <param name="agent">The <see cref="KnowledgeManagementAgent"/> agent.</param>
    /// <param name="explodedObjects">A dictionary of objects retrieved from various object ids related to the agent. For more details see <see cref="LLMCompletionRequest.Objects"/> .</param>
    /// <param name="callContext">The call context of the request being handled.</param>
    /// <param name="orchestrationService"></param>
    /// <param name="logger">The logger used for logging.</param>
    /// <param name="resourceProviderServices">The dictionary of <see cref="IResourceProviderService"/></param>
    /// <param name="dataSourceAccessDenied">Inidicates that access was denied to all underlying data sources.</param>
    public class KnowledgeManagementOrchestration(
        KnowledgeManagementAgent agent,
        Dictionary<string, object> explodedObjects,
        ICallContext callContext,
        ILLMOrchestrationService orchestrationService,
        ILogger<OrchestrationBase> logger,
        Dictionary<string, IResourceProviderService> resourceProviderServices,
        bool dataSourceAccessDenied) : OrchestrationBase(orchestrationService)
    {
        private readonly KnowledgeManagementAgent _agent = agent;
        private readonly Dictionary<string, object> _explodedObjects = explodedObjects;
        private readonly ICallContext _callContext = callContext;
        private readonly ILogger<OrchestrationBase> _logger = logger;
        private readonly bool _dataSourceAccessDenied = dataSourceAccessDenied;

        private readonly IResourceProviderService _attachmentResourceProvider =
            resourceProviderServices[ResourceProviderNames.FoundationaLLM_Attachment];
        private readonly IResourceProviderService _azureOpenAIResourceProvider =
            resourceProviderServices[ResourceProviderNames.FoundationaLLM_AzureOpenAI];

        /// <inheritdoc/>
        public override async Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest)
        {
            if (_dataSourceAccessDenied)
                return new CompletionResponse
                {
                    OperationId = completionRequest.OperationId,
                    Completion = "I have no knowledge that can be used to answer this question.",
                    UserPrompt = completionRequest.UserPrompt!,
                    AgentName = _agent.Name
                };

            if (_agent.ExpirationDate.HasValue && _agent.ExpirationDate.Value < DateTime.UtcNow)
                return new CompletionResponse
                {
                    OperationId = completionRequest.OperationId,
                    Completion = $"The requested agent, {_agent.Name}, has expired and is unable to respond.",
                    UserPrompt = completionRequest.UserPrompt!,
                    AgentName = _agent.Name
                };

            var result = await _orchestrationService.GetCompletion(
                instanceId,
                new LLMCompletionRequest
                {
                    OperationId = completionRequest.OperationId,
                    UserPrompt = completionRequest.UserPrompt!,
                    MessageHistory = completionRequest.MessageHistory,
                    Attachments = completionRequest.Attachments == null ? [] : await GetAttachmentPaths(instanceId, completionRequest.Attachments),
                    Agent = _agent,
                    Objects = _explodedObjects
                });

            if (result.Citations != null)
            {
                result.Citations = result.Citations
                    .GroupBy(c => c.Filepath)
                    .Select(g => g.First())
                    .ToArray();
            }

            return new CompletionResponse
            {
                OperationId = completionRequest.OperationId,
                Completion = result.Completion!,
                UserPrompt = completionRequest.UserPrompt!,
                Citations = result.Citations,
                FullPrompt = result.FullPrompt,
                PromptTemplate = result.PromptTemplate,
                AgentName = result.AgentName,
                PromptTokens = result.PromptTokens,
                CompletionTokens = result.CompletionTokens,
            };
        }

        private async Task<List<AttachmentProperties>> GetAttachmentPaths(string instanceId, List<string> attachmentObjectIds)
        {
            var attachments = attachmentObjectIds
                .ToAsyncEnumerable()
                .SelectAwait(async x => await _attachmentResourceProvider.GetResource<AttachmentFile>(x, _callContext.CurrentUserIdentity!));

            var fileUserContextName = $"{_callContext.CurrentUserIdentity!.UPN?.NormalizeUserPrincipalName() ?? _callContext.CurrentUserIdentity!.UserId}-assistant-{instanceId.ToLower()}";
            var fileUserContex = await _azureOpenAIResourceProvider.GetResource<FileUserContext>(
                $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{AzureOpenAIResourceTypeNames.FileUserContexts}/{fileUserContextName}",
                _callContext.CurrentUserIdentity!);

            List<AttachmentProperties> result = [];
            await foreach (var attachment in attachments)
                result.Add(new AttachmentProperties
                {
                    OriginalFileName = attachment.OriginalFileName,
                    ContentType = attachment.ContentType!,
                    Provider = attachment.SecondaryProvider ?? ResourceProviderNames.FoundationaLLM_Attachment,
                    ProviderFileName = string.IsNullOrWhiteSpace(attachment.SecondaryProvider)
                        ? attachment.Path
                        : fileUserContex.Files[attachment.ObjectId!].OpenAIFileId!,
                    ProviderStorageAccountName = string.IsNullOrWhiteSpace(attachment.SecondaryProvider)
                        ? _attachmentResourceProvider.StorageAccountName
                        : null
                });

            return result;
        }
    }
}
