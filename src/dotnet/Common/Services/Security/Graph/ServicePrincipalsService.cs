using FoundationaLLM.Common.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Services.Security.Graph
{
    /// <summary>
    /// Represents a service for managing service principals in Microsoft Graph.
    /// </summary>
    public class ServicePrincipalsService : GraphPrincipalWithGroupsBase, IGraphPrincipalWithGroups
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServicePrincipalsService"/> class.
        /// </summary>
        /// <param name="graphServiceClient">The GraphServiceClient instance.</param>
        public ServicePrincipalsService(GraphServiceClient graphServiceClient) : base(graphServiceClient) { }

        /// <inheritdoc/>
        public async Task<List<Group>> GetGroups(string principalId)
        {
            var groups = await _graphServiceClient.ServicePrincipals[principalId].GetMemberGroups.PostAsGetMemberGroupsPostResponseAsync(
                new Microsoft.Graph.ServicePrincipals.Item.GetMemberGroups.GetMemberGroupsPostRequestBody
                {
                    SecurityEnabledOnly = true
                }
            ).ConfigureAwait(false);

            return groups?.Value == null ? []
                : groups.Value.Select(group => new Group { Id = group }).ToList();
        }
    }
}
