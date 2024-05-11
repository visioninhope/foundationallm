using FoundationaLLM.Common.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Services.Security.Graph
{
    public class ServicePrincipals : GraphPrincipalWithGroupsBase, IGraphPrincipalWithGroups
    {
        public ServicePrincipals(GraphServiceClient graphServiceClient) : base(graphServiceClient) { }

        public async Task<List<Group>> GetGroups(string principalId)
        {
            var groups = await _graphServiceClient.ServicePrincipals[principalId].GetMemberGroups.PostAsGetMemberGroupsPostResponseAsync(
                new Microsoft.Graph.ServicePrincipals.Item.GetMemberGroups.GetMemberGroupsPostRequestBody
                {
                    SecurityEnabledOnly = true
                }
            );
            if (groups == null || groups.Value == null)
                return [];
            return groups.Value.Select(group => new Group { Id = group }).ToList();
        }
    }
}
