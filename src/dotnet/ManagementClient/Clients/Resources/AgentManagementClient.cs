using FoundationaLLM.Client.Management.Interfaces;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Client.Management.Clients.Resources
{
    internal class AgentManagementClient(IManagementRESTClient managementRestClient) : IAgentManagementClient
    {
        /// <summary>
        /// Retrieves all agent resources.
        /// </summary>
        /// <returns>All agent resources to which the caller has access and which have not been marked as deleted.</returns>
        public async Task<List<AgentBase>> GetAgentsAsync() => await managementRestClient.Resources.GetResourcesAsync<List<AgentBase>>(
                ResourceProviderNames.FoundationaLLM_Agent,
                AgentResourceTypeNames.Agents
            );

        /// <summary>
        /// Checks the availability of a resource name for an agent. If the name is available, the
        /// <see cref="ResourceNameCheckResult.Status"/> value will be "Allowed". If the name is
        /// not available, the <see cref="ResourceNameCheckResult.Status"/> value will be "Denied" and
        /// the <see cref="ResourceNameCheckResult.Message"/> will explain the reason why. Typically,
        /// a denied name is due to a name conflict with an existing agent or an agent that was
        /// deleted but not purged.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ResourceNameCheckResult> CheckAgentNameAsync(ResourceName resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName.Name) || string.IsNullOrWhiteSpace(resourceName.Type))
            {
                throw new ArgumentException("Resource name and type must be provided.");
            }

            return await managementRestClient.Resources.ExecuteResourceActionAsync<ResourceNameCheckResult>(
                ResourceProviderNames.FoundationaLLM_Agent,
                $"{AgentResourceTypeNames.Agents}/checkname",
                resourceName
            );
        }

        /// <summary>
        /// Purges a deleted agent by its name. This action is irreversible.
        /// </summary>
        /// <param name="agentName"></param>
        /// <returns></returns>
        public async Task<ResourceProviderActionResult> PurgeAgentAsync(string agentName) =>
            await managementRestClient.Resources.ExecuteResourceActionAsync<ResourceProviderActionResult>(
                           ResourceProviderNames.FoundationaLLM_Agent,
                           $"{AgentResourceTypeNames.Agents}/{AgentResourceProviderActions.Purge}",
                           new { }
                );

        /// <summary>
        /// Retrieves a specific agent by name.
        /// </summary>
        /// <param name="agentName">The name of the agent resource to retrieve.</param>
        /// <returns></returns>
        public async Task<AgentBase> GetAgentAsync(string agentName)
        {
            var result = await managementRestClient.Resources.GetResourcesAsync<List<AgentBase>>(
                ResourceProviderNames.FoundationaLLM_Agent,
                $"{AgentResourceTypeNames.Agents}/{agentName}"
            );

            if (result == null || result.Count == 0)
            {
                throw new Exception($"Agent '{agentName}' not found.");
            }

            var agent = result[0];

            return agent;
        }

        /// <summary>
        /// Upserts an agent resource. If an agent does not exist, it will be created. If an agent
        /// does exist, it will be updated.
        /// </summary>
        /// <param name="agent">The agent resource to create or update.</param>
        /// <returns>Returns a <see cref="ResourceProviderUpsertResult"/>, which contains the
        /// Object ID of the resource.</returns>
        public async Task<ResourceProviderUpsertResult> UpsertAgentAsync(AgentBase agent) => await managementRestClient.Resources.UpsertResourceAsync(
                ResourceProviderNames.FoundationaLLM_Agent,
              $"{AgentResourceTypeNames.Agents}/{agent.Name}",
                agent
            );

        /// <summary>
        /// Deletes an agent resource by name. Please note that all deletes are soft deletes. The
        /// resource will be marked as deleted but not purged. To permanently remove a resource,
        /// execute the <see cref="PurgeAgent"/> method with the same name.
        /// </summary>
        /// <param name="agentName">The name of the agent resource to delete.</param>
        /// <returns></returns>
        public async Task DeleteAgentAsync(string agentName) => await managementRestClient.Resources.DeleteResourceAsync(
                ResourceProviderNames.FoundationaLLM_Agent,
                $"{AgentResourceTypeNames.Agents}/{agentName}"
            );
    }
}
