using FoundationaLLM.Common.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Services.Security.Graph
{

    /// <summary>
    /// Represents a service for querying the Microsoft Graph API for group membership of a user.
    /// </summary>
    public class UsersService : GraphPrincipalWithGroupsBase, IGraphPrincipalWithGroups
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="graphServiceClient">The Microsoft Graph API client.</param>
        public UsersService(GraphServiceClient graphServiceClient) : base(graphServiceClient) { }

        /// <inheritdoc/>
        public async Task<List<Group>> GetGroups(string principalId)
        {
            var groups = await _graphServiceClient.Users[principalId].TransitiveMemberOf.GraphGroup.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Top = 500;
            }).ConfigureAwait(false);

            var groupMembership = new List<Group>();

            while (groups?.Value != null)
            {
                groupMembership.AddRange(groups.Value);

                // Invoke paging if required.
                if (!string.IsNullOrEmpty(groups.OdataNextLink))
                {
                    groups = await _graphServiceClient.Users[principalId].TransitiveMemberOf.GraphGroup
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
