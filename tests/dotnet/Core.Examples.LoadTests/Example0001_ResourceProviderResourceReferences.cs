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

            var simulatedUsersCount = 20;

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
                    i,
                    simulatedUsersCount,
                    resourceProvidersHosts[i],
                    ServiceProviders[i]))
                );
        }

        private async Task SimulateServiceHostLoad(
            int hostId,
            int simulatedUsersCount,
            LoadTestResourceProviders resourceProviders,
            IServiceProvider serviceProvider)
        {
            var endpoint = Environment.GetEnvironmentVariable("FOUNDATIONALLM_OPENAI_ASSISTANT_ENDPOINT")!;
            var modelDeploymentName = Environment.GetEnvironmentVariable("FOUNDATIONALLM_OPENAI_ASSISTANT_MODEL_NAME")!;
            var prompt = "What was the average population of Canada in the 1950s?";
            var fileName = "world_demographics_cp.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var instanceSettings = serviceProvider.GetRequiredService<IOptions<InstanceSettings>>().Value;
            var instanceId = instanceSettings.Id;
            var userIdentities = GetUserIdentities(hostId, simulatedUsersCount);
            var userContexts = userIdentities
                .Select(userIdentity => {
                    var newGuid = Guid.NewGuid().ToString();
                    return new AssistantUserContext
                    {
                        Name = $"{userIdentity.UPN!.NormalizeUserPrincipalName()}-assistant-{instanceId.ToLower()}",
                        UserPrincipalName = userIdentity.UPN!,
                        Endpoint = endpoint,
                        ModelDeploymentName = modelDeploymentName,
                        Prompt = prompt,
                        Conversations = new Dictionary<string, ConversationMapping>
                        {
                            { newGuid, new ConversationMapping { FoundationaLLMSessionId = newGuid } }
                        }
                    };
                })
                .ToList();

            byte[] fileData = File.ReadAllBytes(fileName);

            var uploadedFiles = await Task.WhenAll(
                userIdentities.Select(
                    userIdentity => SimulateFoundationaLLMFileUploads(
                        instanceId,
                        new AttachmentFile
                        {
                            Name = $"a-{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}",
                            OriginalFileName = fileName,
                            DisplayName = fileName,
                            Content = fileData,
                            ContentType = mimeType,
                            SecondaryProvider = "FoundationaLLM.AzureOpenAI"
                        },
                        resourceProviders.AttachmentResourceProvider,
                        userIdentity
                    )
                )
            );

            await Task.WhenAll(
                uploadedFiles.Select(
                    file => SimulateAzureOpenAIFileUploads(
                        instanceId,
                        new FileUserContext
                        {
                            Name = file.uploadedFileName,
                            UserPrincipalName = file.userIdentity.UPN!,
                            Endpoint = endpoint,
                            AssistantUserContextName = file.assistantUserContextName,
                            Files = new Dictionary<string, FileMapping>
                            {
                                {
                                    file.uploadedFileName,
                                    new FileMapping
                                    {
                                        FoundationaLLMObjectId = file.fileObjectId,
                                        OriginalFileName = fileName,
                                        ContentType = mimeType
                                    }
                                }
                            }
                        },
                        resourceProviders.AzureOpenAIResourceProvider,
                        file.userIdentity
                    )
                )
            );

            var assistantUserContexts = await Task.WhenAll(
                Enumerable.Range(0, userContexts.Count)
                    .Select(i => SimulateAssistantUserContextCreation(
                        instanceId,
                        userContexts[i],
                        resourceProviders.AzureOpenAIResourceProvider,
                        userIdentities[i]))
                    .ToList()
            );

            // User creates a new chat session (update existing context)
            userContexts.ForEach(context => { 
                var newGuid = Guid.NewGuid().ToString();
                context.Conversations.Add(newGuid, new ConversationMapping { FoundationaLLMSessionId = newGuid });
            });

            await Task.WhenAll(
                Enumerable.Range(0, assistantUserContexts.Count())
                    .Select(
                        i => SimulateAssistantUserContextUpdate(
                            resourceProviders.AzureOpenAIResourceProvider,
                            assistantUserContexts[i].ObjectId!,
                            userContexts[i],
                            userIdentities[i]
                        )
                    )
            );
        }

        private async Task<AssistantUserContextUpsertResult> SimulateAssistantUserContextCreation(
            string instanceId,
            AssistantUserContext assistantUserContext,
            IResourceProviderService resourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            return await resourceProvider.CreateOrUpdateResource<AssistantUserContext, AssistantUserContextUpsertResult>(
                instanceId,
                assistantUserContext,
                AzureOpenAIResourceTypeNames.AssistantUserContexts,
                userIdentity);
        }

        private async Task SimulateAssistantUserContextUpdate(
            IResourceProviderService resourceProvider,
            string resourcePath,
            AssistantUserContext assistantUserContext,
            UnifiedUserIdentity userIdentity)
        {
            await resourceProvider.UpsertResourceAsync<AssistantUserContext, AssistantUserContextUpsertResult>(
                resourcePath,
                assistantUserContext,
                userIdentity
            );
        }

        private async Task<(string uploadedFileName, UnifiedUserIdentity userIdentity, string fileObjectId, string assistantUserContextName)> SimulateFoundationaLLMFileUploads(
            string instanceId,
            AttachmentFile attachmentFile,
            IResourceProviderService resourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            var objectId = (await resourceProvider.UpsertResourceAsync<AttachmentFile, ResourceProviderUpsertResult>(
                $"/instances/{instanceId}/providers/{ResourceProviderNames.FoundationaLLM_Attachment}/attachments/{attachmentFile.Name}",
                attachmentFile,
                userIdentity
            )).ObjectId!;
            return (attachmentFile.Name, userIdentity, objectId, $"{userIdentity.UPN!.NormalizeUserPrincipalName()}-assistant-{instanceId.ToLower()}");
        }

        private async Task SimulateAzureOpenAIFileUploads(
            string instanceId,
            FileUserContext fileUserContext,
            IResourceProviderService resourceProvider,
            UnifiedUserIdentity userIdentity)
        {
            await resourceProvider.CreateOrUpdateResource<FileUserContext, FileUserContextUpsertResult>(
                instanceId,
                fileUserContext,
                AzureOpenAIResourceTypeNames.FileUserContexts,
                userIdentity
            );
        }

        private List<UnifiedUserIdentity> GetUserIdentities(
            int hostId,
            int simulatedUsersCount)
        {
            return Enumerable.Range(1, 20)
                .Select(i => new UnifiedUserIdentity
                {
                    GroupIds = ["00000000-0000-0000-0000-000000000001"],
                    UserId = $"00000000-0000-0000-{hostId:D4}-{i:D12}",
                    Username = $"load_test_user_{hostId:D3}_{i:D3}@solliance.net",
                    Name = $"Load Test User {hostId:D3}-{i:D3}",
                    UPN = $"load_test_user_{hostId:D3}_{i:D3}@solliance.net"
                })
                .ToList();
        }
    }
}