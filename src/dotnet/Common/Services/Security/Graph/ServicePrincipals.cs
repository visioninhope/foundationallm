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
            var groups = await _graphServiceClient.ServicePrincipals[principalId].TransitiveMemberOf.GraphGroup.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Top = 500;
            }).ConfigureAwait(false);

            var groupMembership = new List<Group>();

            while (groups?.Value != null)
            {
                foreach (var group in groups.Value)
                {
                    groupMembership.Add(group);
                }

                // Invoke paging if required.
                if (!string.IsNullOrEmpty(groups.OdataNextLink))
                {
                    groups = await _graphServiceClient.ServicePrincipals[principalId].TransitiveMemberOf.GraphGroup
                        .WithUrl(groups.OdataNextLink)
                        .GetAsync();
                }
                else
                {
                    break;
                }
            }

            return groupMembership;
        }
    }
}
