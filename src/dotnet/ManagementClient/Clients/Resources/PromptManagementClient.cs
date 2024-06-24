using FoundationaLLM.Client.Management.Interfaces;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Prompt;

namespace FoundationaLLM.Client.Management.Clients.Resources
{
    internal class PromptManagementClient(IManagementRESTClient managementRestClient) : IPromptManagementClient
    {
        /// <inheritdoc/>
        public async Task<List<PromptBase>> GetPromptsAsync() => await managementRestClient.Resources.GetResourcesAsync<List<PromptBase>>(
                ResourceProviderNames.FoundationaLLM_Prompt,
                PromptResourceTypeNames.Prompts
            );

        /// <inheritdoc/>
        public async Task<ResourceNameCheckResult> CheckPromptNameAsync(ResourceName resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName.Name) || string.IsNullOrWhiteSpace(resourceName.Type))
            {
                throw new ArgumentException("Resource name and type must be provided.");
            }

            return await managementRestClient.Resources.ExecuteResourceActionAsync<ResourceNameCheckResult>(
                ResourceProviderNames.FoundationaLLM_Prompt,
                $"{PromptResourceTypeNames.Prompts}/checkname",
                resourceName
            );
        }

        /// <inheritdoc/>
        public async Task<ResourceProviderActionResult> PurgePromptAsync(string promptName) =>
            await managementRestClient.Resources.ExecuteResourceActionAsync<ResourceProviderActionResult>(
                           ResourceProviderNames.FoundationaLLM_Agent,
                           $"{AgentResourceTypeNames.Agents}/{AgentResourceProviderActions.Purge}",
                           new { }
                );

        /// <inheritdoc/>
        public async Task<PromptBase> GetPromptAsync(string promptName)
        {
            var result = await managementRestClient.Resources.GetResourcesAsync<List<PromptBase>>(
                ResourceProviderNames.FoundationaLLM_Prompt,
                $"{PromptResourceTypeNames.Prompts}/{promptName}"
            );

            if (result == null || result.Count == 0)
            {
                throw new Exception($"Prompt '{promptName}' not found.");
            }

            var agent = result[0];

            return agent;
        }

        /// <inheritdoc/>
        public async Task<ResourceProviderUpsertResult> UpsertPromptAsync(PromptBase prompt) => await managementRestClient.Resources.UpsertResourceAsync(
            ResourceProviderNames.FoundationaLLM_Prompt,
            $"{PromptResourceTypeNames.Prompts}/{prompt.Name}",
                prompt
            );

        /// <inheritdoc/>
        public async Task DeletePromptAsync(string promptName) => await managementRestClient.Resources.DeleteResourceAsync(
                ResourceProviderNames.FoundationaLLM_Prompt,
                $"{PromptResourceTypeNames.Prompts}/{promptName}"
            );
    }
}
