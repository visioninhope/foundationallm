using FoundationaLLM.Client.Management.Interfaces;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Client.Management.Clients.Resources
{
    internal class AgentManagementClient(IManagementRESTClient managementRestClient)
    {
        public async Task<List<AgentBase>> GetAgentsAsync() => await managementRestClient.Resources.GetResourcesAsync<List<AgentBase>>(
                ResourceProviderNames.FoundationaLLM_Agent,
                AgentResourceTypeNames.Agents
            );

        /// <summary>
        /// Retrieves a specific agent by name.
        /// </summary>
        /// <param name="agentName">The name of the agent resource.</param>
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

        public async Task<ResourceProviderUpsertResult> UpsertAgentAsync(AgentBase agent) => await managementRestClient.Resources.UpsertResourceAsync(
                ResourceProviderNames.FoundationaLLM_Agent,
              $"{AgentResourceTypeNames.Agents}/{agent.Name}",
                agent
            );

        public async Task DeleteAgentAsync(string agentName) => await managementRestClient.Resources.DeleteResourceAsync(
                ResourceProviderNames.FoundationaLLM_Agent,
                $"{AgentResourceTypeNames.Agents}/{agentName}"
            );
    }
}
