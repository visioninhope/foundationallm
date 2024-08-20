using Core.Examples.LoadTests.Data;
using Core.Examples.LoadTests.ResourceProviders;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Models.ResourceProviders.AzureOpenAI;
using FoundationaLLM.Core.Examples.LoadTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples.LoadTests
{
    /// <summary>
    /// Runs load tests on resource provider resource references.
    /// </summary>
    public class Example0001_ResourceProviderResourceReferences : BaseTest, IClassFixture<LoadTestFixture>
    {
        public Example0001_ResourceProviderResourceReferences(ITestOutputHelper output, LoadTestFixture fixture)
			: base(output, fixture.ServiceProviders)
        {
        }

        [Fact]
        public async Task RunAsync()
        {
            WriteLine("============ FoundationaLLM Resource Provider Load Test ============");

            // Get resource providers in all DI containers.
            var resourceProvidersHosts = ServiceProviders
                .Select(sp => new LoadTestResourceProviders(sp, Output))
                .ToList();

            // Initialize all resource providers.
            await Task.WhenAll(resourceProvidersHosts
                .Select(rps => rps.InitializeAll()));

            await Task.WhenAll(
                Enumerable.Range(0, resourceProvidersHosts.Count)
                .Select(i => SimulateServiceHostLoad(
                    i + 1,
                    resourceProvidersHosts[i],
                    ServiceProviders[i]))
                );
        }

        private async Task SimulateServiceHostLoad(
            int hostId,
            LoadTestResourceProviders resourceProviders,
            IServiceProvider serviceProvider)
        {
            var instanceSettings = serviceProvider.GetRequiredService<IOptions<InstanceSettings>>().Value;

            var userIdentities = LoadTestData.GetUserIdentities(hostId);

            var results = await Task.WhenAll(
               userIdentities
               .Select(userIdentity => SimulateAttachmentFileUploadAndFileUserContextCreation(
                   instanceSettings.Id,
                   resourceProviders.AttachmentResourceProvider,
                   resourceProviders.AzureOpenAIResourceProvider,
                   userIdentity)));

            await Task.WhenAll(
                userIdentities
                .Select(userIdentity => SimulateAssistantUserContextCreation(
                    instanceSettings.Id,
                    resourceProviders.AttachmentResourceProvider,
                    resourceProviders.AzureOpenAIResourceProvider,
                    userIdentity,
                    results.Single(x => x.Item1 == userIdentity.UserId!).Item2)));

            //await Task.WhenAll(
            //    userIdentities
            //    .Select(userIdentity =>
            //        serviceProvider.GetRequiredService<IAuthorizationService>().ProcessAuthorizationRequest(
            //        instanceSettings.Id,
            //        "FoundationaLLM.AzureOpenAI/assistantUserContexts/write", 
            //        [$"/instances/{instanceSettings.Id}/providers/FoundationaLLM.AzureOpenAI/assistantUserContexts/{userIdentity.UPN!.NormalizeUserPrincipalName()}-assistant-{instanceSettings.Id.ToLower()}"], 
            //        userIdentity)));
        }

        private async Task SimulateAssistantUserContextCreation(
            string instanceId,
            IResourceProviderService attachmentResourceProvider,
            IResourceProviderService azureOpenAIResourceProvider,
            UnifiedUserIdentity userIdentity,
            List<string> attachmentObjectIds)
        {
            var assistantUserContextName = $"{userIdentity.UPN!.NormalizeUserPrincipalName()}-assistant-{instanceId.ToLower()}";

            var sessionId = Guid.NewGuid().ToString();

            var assistantUserContext = new AssistantUserContext
            {
                Name = assistantUserContextName,
                UserPrincipalName = userIdentity.UPN!,
                Endpoint = "endpoint_placeholder",
                ModelDeploymentName = "model_placeholder",
                Prompt = "prompt_placeholder",
                Conversations = new()
                {
                    {
                        sessionId!,
                        new ConversationMapping
                        {
                            FoundationaLLMSessionId = sessionId!
                        }
                    }
                }
            };

            var exists = await azureOpenAIResourceProvider.ResourceExists(
                instanceId,
                assistantUserContextName,
                AzureOpenAIResourceTypeNames.AssistantUserContexts,
                userIdentity);

            if (!exists)
                _ = await azureOpenAIResourceProvider.CreateOrUpdateResource<AssistantUserContext, AssistantUserContextUpsertResult>(
                instanceId,
                assistantUserContext,
                AzureOpenAIResourceTypeNames.AssistantUserContexts,
                userIdentity);

            var attachments = attachmentObjectIds
                 .ToAsyncEnumerable()
                 .SelectAwait(async x => await attachmentResourceProvider.GetResource<AttachmentFile>(x, userIdentity!));

            var fileUserContextObjectId = $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{AzureOpenAIResourceTypeNames.FileUserContexts}/"
                + $"{userIdentity!.UPN?.NormalizeUserPrincipalName() ?? userIdentity!.UserId}-file-{instanceId.ToLower()}";

            var fileUserContext = await azureOpenAIResourceProvider.GetResource<FileUserContext>(
                fileUserContextObjectId,
                userIdentity!);

            // TODO: update fileUserContext.Files
            //var result = contentItems.Select(ci => TransformContentItem(ci, newFileMappings)).ToList();
            //foreach (var fileMapping in newFileMappings)
            //{
            //    fileUserContext.Files.TryAdd(fileMapping.FoundationaLLMObjectId, fileMapping);
            //}

            _ = await azureOpenAIResourceProvider.UpsertResourceAsync<FileUserContext, FileUserContextUpsertResult>(
                fileUserContextObjectId,
                fileUserContext,
                userIdentity!);
        }

        private async Task<(string, List<string>)> SimulateAttachmentFileUploadAndFileUserContextCreation(
            string instanceId,
            IResourceProviderService attachmentResourceProvider,
            IResourceProviderService azureOpenAIResourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            var attachmentFiles = LoadTestData.GetAttachmentFiles(userIdentity.UserId!);

            var attachmentObjectIds = await Task.WhenAll(
               attachmentFiles
               .Select(attachmentFile => UploadAttachmentFileAndUpsertFileUserContext(
                   instanceId,
                   attachmentResourceProvider,
                   azureOpenAIResourceProvider,
                   attachmentFile,
                   userIdentity)));

            return (userIdentity.UserId!, [.. attachmentObjectIds]);
        }

        private async Task<string> UploadAttachmentFileAndUpsertFileUserContext(string instanceId,
            IResourceProviderService attachmentResourceProvider,
            IResourceProviderService azureOpenAIResourceProvider,
            AttachmentFile attachmentFile,
            UnifiedUserIdentity userIdentity)
        {
            var attachmentResult = await attachmentResourceProvider.UpsertResourceAsync<AttachmentFile, ResourceProviderUpsertResult>(
                    $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_Attachment}/attachments/{attachmentFile.Name}",
                attachmentFile,
                    userIdentity);

            var fileUserContext = LoadTestData.GetFileUserContext(userIdentity.Username!, instanceId, attachmentResult.ObjectId!, attachmentFile);

            var fileResult = await azureOpenAIResourceProvider.UpsertResourceAsync<FileUserContext, ResourceProviderUpsertResult>(
                $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{AzureOpenAIResourceTypeNames.FileUserContexts}/{fileUserContext.Name}",
                fileUserContext,
                userIdentity);

            _ = await attachmentResourceProvider.GetResource<AttachmentFile>(attachmentResult.ObjectId!, userIdentity);

            return attachmentResult.ObjectId!;
        }
    }
}