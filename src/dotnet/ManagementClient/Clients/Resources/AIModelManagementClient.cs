using FoundationaLLM.Client.Management.Interfaces;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.AIModel;

namespace FoundationaLLM.Client.Management.Clients.Resources
{
    internal class AIModelManagementClient(IManagementRESTClient managementRestClient) : IAIModelManagementClient
    {
        /// <inheritdoc/>
        public async Task<ResourceProviderUpsertResult> UpsertAIModel(AIModelBase aiModel) =>
            await managementRestClient.Resources.UpsertResourceAsync(
                ResourceProviderNames.FoundationaLLM_AIModel,
                $"{AIModelResourceTypeNames.AIModels}/{aiModel.Name}",
                aiModel
            );
    }
}
