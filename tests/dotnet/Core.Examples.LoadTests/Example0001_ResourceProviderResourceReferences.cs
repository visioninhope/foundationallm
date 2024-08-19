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
        private const int SimulatedUsersCount = 20;
        private const int SimulatedFileCount = 10;

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

            await Task.WhenAll(
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
                    resourceProviders.AzureOpenAIResourceProvider,
                    userIdentity)));
        }

        private async Task SimulateAssistantUserContextCreation(
            string instanceId,
            IResourceProviderService resourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            var assistantUserContextName = $"{userIdentity.UPN!.NormalizeUserPrincipalName()}-assistant-{instanceId.ToLower()}";
            var assistantUserContext = new AssistantUserContext
            {
                Name = assistantUserContextName,
                UserPrincipalName = userIdentity.UPN!,
                Endpoint = "endpoint_placeholder",
                ModelDeploymentName = "model_placeholder",
                Prompt = "prompt_placeholder",
            };

            _ = await resourceProvider.CreateOrUpdateResource<AssistantUserContext, AssistantUserContextUpsertResult>(
                instanceId,
                assistantUserContext,
                AzureOpenAIResourceTypeNames.AssistantUserContexts,
                userIdentity);
        }

        private async Task SimulateAttachmentFileUploadAndFileUserContextCreation(
            string instanceId,
            IResourceProviderService attachmentResourceProvider,
            IResourceProviderService azureOpenAIResourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            var attachmentFiles = LoadTestData.GetAttachmentFiles(userIdentity.UserId!);

            await Task.WhenAll(
               attachmentFiles
               .Select(attachmentFile => UploadAttachmentFileAndUpsertFileUserContext(
                   instanceId,
                   attachmentResourceProvider,
                   azureOpenAIResourceProvider,
                   attachmentFile,
                   userIdentity)));
        }

        private async Task UploadAttachmentFileAndUpsertFileUserContext(string instanceId,
            IResourceProviderService attachmentResourceProvider,
            IResourceProviderService azureOpenAIResourceProvider,
            AttachmentFile attachmentFile,
            UnifiedUserIdentity userIdentity)
        {
            var result = await attachmentResourceProvider.UpsertResourceAsync<AttachmentFile, ResourceProviderUpsertResult>(
                    $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_Attachment}/attachments/{attachmentFile.Name}",
                attachmentFile,
                    userIdentity);

            var fileUserContext = LoadTestData.GetFileUserContext(userIdentity.Username!, instanceId, result.ObjectId!, attachmentFile);

            _ = await azureOpenAIResourceProvider.UpsertResourceAsync<FileUserContext, ResourceProviderUpsertResult>(
                $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_AzureOpenAI}/{AzureOpenAIResourceTypeNames.FileUserContexts}/{fileUserContext.Name}",
                fileUserContext,
                userIdentity);

            _ = await attachmentResourceProvider.GetResource<AttachmentFile>(result.ObjectId!, userIdentity);
        }
    }
}