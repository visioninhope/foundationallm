using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration.Request;
using FoundationaLLM.Common.Models.Orchestration.Response;
using FoundationaLLM.Common.Models.Orchestration.Response.OpenAI;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Models.ResourceProviders.AzureOpenAI;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace FoundationaLLM.Orchestration.Core.Orchestration
{
    /// <summary>
    /// Knowledge Management orchestration.
    /// </summary>
    /// <remarks>
    /// Constructor for default agent.
    /// </remarks>
    /// <param name="instanceId">The FoundationaLLM instance ID.</param>
    /// <param name="agent">The <see cref="KnowledgeManagementAgent"/> agent.</param>
    /// <param name="explodedObjects">A dictionary of objects retrieved from various object ids related to the agent. For more details see <see cref="LLMCompletionRequest.Objects"/> .</param>
    /// <param name="callContext">The call context of the request being handled.</param>
    /// <param name="orchestrationService"></param>
    /// <param name="logger">The logger used for logging.</param>
    /// <param name="resourceProviderServices">The dictionary of <see cref="IResourceProviderService"/></param>
    /// <param name="dataSourceAccessDenied">Inidicates that access was denied to all underlying data sources.</param>
    public class KnowledgeManagementOrchestration(
        string instanceId,
        KnowledgeManagementAgent agent,
        Dictionary<string, object> explodedObjects,
        ICallContext callContext,
        ILLMOrchestrationService orchestrationService,
        ILogger<OrchestrationBase> logger,
        Dictionary<string, IResourceProviderService> resourceProviderServices,
        bool dataSourceAccessDenied) : OrchestrationBase(orchestrationService)
    {
        private readonly string _instanceId = instanceId;
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
                    Attachments = await GetAttachmentPaths(instanceId, completionRequest.Attachments),
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
                Content = result.Content?.Select(c => TransformContentItem(c)!).ToList(),
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
            if (attachmentObjectIds.Count == 0)
                return [];

            var attachments = attachmentObjectIds
                .ToAsyncEnumerable()
                .SelectAwait(async x => await _attachmentResourceProvider.GetResource<AttachmentFile>(x, _callContext.CurrentUserIdentity!));

            var fileUserContextName = $"{_callContext.CurrentUserIdentity!.UPN?.NormalizeUserPrincipalName() ?? _callContext.CurrentUserIdentity!.UserId}-file-{instanceId.ToLower()}";
            var fileUserContext = await _azureOpenAIResourceProvider.GetResource<FileUserContext>(
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
                        : fileUserContext.Files[attachment.ObjectId!].OpenAIFileId!,
                    ProviderStorageAccountName = string.IsNullOrWhiteSpace(attachment.SecondaryProvider)
                        ? _attachmentResourceProvider.StorageAccountName
                        : null
                });

            return result;
        }

        private MessageContentItemBase TransformContentItem(MessageContentItemBase contentItem) =>
            contentItem.AgentCapabilityCategory switch
            {
                AgentCapabilityCategoryNames.OpenAIAssistants => TransformOpenAIAssistantsContentItem(contentItem),
                AgentCapabilityCategoryNames.FoundationaLLMKnowledgeManagement => TransformFoundationaLLMKnowledgeManagementContentItem(contentItem),
                _ => throw new OrchestrationException($"The agent capability category {contentItem.AgentCapabilityCategory} is not supported.")
            };

        #region OpenAI Assistants content items

        private MessageContentItemBase TransformOpenAIAssistantsContentItem(MessageContentItemBase contentItem) =>
            contentItem switch
            {
                OpenAIImageFileMessageContentItem openAIImageFile => TransformOpenAIAssistantsImageFile(openAIImageFile),
                OpenAITextMessageContentItem openAITextMessage => TransformOpenAIAssistantsTextMessage(openAITextMessage),
                _ => throw new OrchestrationException($"The content item type {contentItem.GetType().Name} is not supported.")
            };

        private OpenAIImageFileMessageContentItem TransformOpenAIAssistantsImageFile(OpenAIImageFileMessageContentItem openAIImageFile)
        {
            openAIImageFile.FileUrl = $"/instances/{_instanceId}/files/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{openAIImageFile.FileId}";
            return openAIImageFile;
        }

        private OpenAIFilePathContentItem TransformOpenAIAssistantsFilePath(OpenAIFilePathContentItem openAIFilePath)
        {
            openAIFilePath.FileUrl = $"/instances/{_instanceId}/files/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{openAIFilePath.FileId}";
            return openAIFilePath;
        }

        private OpenAITextMessageContentItem TransformOpenAIAssistantsTextMessage(OpenAITextMessageContentItem openAITextMessage)
        {
            openAITextMessage.Annotations = openAITextMessage.Annotations
                .Select(a => TransformOpenAIAssistantsFilePath(a))
                .ToList();
            var sandboxPlaceholders = openAITextMessage.Annotations.ToDictionary(
                a => a.Text!,
                a => a.FileUrl!);

            var input = openAITextMessage.Value!;
            var regex = new Regex(@"\(sandbox:[^)]*\)");
            var matches = regex.Matches(input);

            if (matches.Count == 0)
                return openAITextMessage;

            Match? previousMatch = null;
            List<string> output = [];

            foreach (Match match in matches)
            {
                var startIndex = previousMatch == null ? 0 : previousMatch.Index + previousMatch.Length;
                output.Add(input.Substring(startIndex, match.Index - startIndex));
                var token = input.Substring(match.Index, match.Length);
                if (sandboxPlaceholders.TryGetValue(token, out var replacement))
                    output.Add(replacement);
                else
                    output.Add(token);

                previousMatch = match;
            }

            output.Add(input.Substring(previousMatch!.Index + previousMatch.Length));

            openAITextMessage.Value = string.Join("", output);
            return openAITextMessage;
        }

        #endregion

        #region FoundationaLLM Knowledge Management content items

        private MessageContentItemBase TransformFoundationaLLMKnowledgeManagementContentItem(MessageContentItemBase contentItem) =>
            contentItem;

        #endregion
    }
}
