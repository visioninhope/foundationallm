using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Interfaces;
using Microsoft.Graph;
using System.Collections.Generic;

namespace FoundationaLLM.Common.Services.Security
{
    /// <summary>
    /// Implements group membership services using the Microsoft Graph API.
    /// </summary>
    public class MicrosoftGraphGroupMembershipService : IGroupMembershipService
    {
        private readonly GraphServiceClient _graphClient = new GraphServiceClient(
            DefaultAuthentication.GetAzureCredential());

        /// <inheritdoc/>
        public async Task<List<string>> GetGroupsForPrincipal(string userPrincipalName)
        {
            var groupMembership = new List<Microsoft.Graph.Models.Group>();
            var groups = await _graphClient.Users[userPrincipalName].TransitiveMemberOf.GraphGroup.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Top = 500;
            }).ConfigureAwait(false);

            while (groups?.Value != null)
            {
                foreach (var group in groups.Value)
                {
                    Console.WriteLine(group.DisplayName);
                    groupMembership.Add(group);
                }

                // Invoke paging if required.
                if (!string.IsNullOrEmpty(groups.OdataNextLink))
                {
                    groups = await _graphClient.Users[userPrincipalName].TransitiveMemberOf.GraphGroup
                        .WithUrl(groups.OdataNextLink)
                        .GetAsync();
                }
                else
                {
                    break;
                }
            }

            return groupMembership.Count == 0
                ? []
                : groupMembership.Where(x => x.Id != null).Select(x => x.Id!).ToList();
        }
    }
}
