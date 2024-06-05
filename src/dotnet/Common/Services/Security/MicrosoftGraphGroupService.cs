using Azure.Search.Documents.Models;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Collections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Groups;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;

namespace FoundationaLLM.Common.Services.Security
{
    /// <summary>
    /// Implements group membership services using the Microsoft Graph API.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MicrosoftGraphGroupService"/> class.
    /// </remarks>
    /// <param name="graphServiceClient">The GraphServiceClient to be used for API interactions.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class MicrosoftGraphGroupService(
        GraphServiceClient graphServiceClient,
        ILogger<MicrosoftGraphGroupService> logger) : IGroupService
    {
        private readonly ILogger<MicrosoftGraphGroupService> _logger = logger;

        /// <inheritdoc/>
        public async Task<List<string>> GetGroupsForPrincipalAsync(string userIdentifier)
        {
            var groups = await graphServiceClient.Users[userIdentifier].TransitiveMemberOf.GraphGroup.GetAsync(requestConfiguration =>
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
                    groups = await graphServiceClient.Users[userIdentifier].TransitiveMemberOf.GraphGroup
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

        /// <inheritdoc/>
        public async Task<GroupAccount> GetUserGroupByIdAsync(string groupId)
        {
            var group = await graphServiceClient.Groups[groupId].GetAsync();

            return new GroupAccount
            {
                Id = group?.Id,
                Name = group?.DisplayName
            };
        }
        /// <inheritdoc/>
        public async Task<PagedResponse<GroupAccount>> GetUserGroupsAsync(AccountQueryParameters queryParams)
        {
            // TODO: Authorize the user to access this endpoint via the AuthorizationService.
            // A user must be assigned the "Role Based Access Control Administrator" role (/providers/FoundationaLLM.Authorization/roleDefinitions/17ca4b59-3aee-497d-b43b-95dd7d916f99) to access this endpoint.

            var pageSize = queryParams.PageSize ?? 100;
            var userGroups = new List<GroupAccount>();

            var currentPage = 1;

            // Retrieve group accounts with filtering and paging options.
            var groupsPage = await graphServiceClient.Groups
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = ["id", "displayName"];
                    requestConfiguration.QueryParameters.Filter = "securityEnabled eq true";
                    if (!string.IsNullOrEmpty(queryParams.Name))
                    {
                        requestConfiguration.QueryParameters.Search = $"\"displayName:{queryParams.Name}\"";
                    }
                    requestConfiguration.QueryParameters.Orderby = ["displayName"];
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Count = true;
                    requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
                });

            // Skip pages until we reach the desired page.
            while (groupsPage?.OdataNextLink != null && currentPage < queryParams.PageNumber)
            {
                groupsPage = await graphServiceClient.Groups
                    .WithUrl(groupsPage.OdataNextLink)
                    .GetAsync();
                currentPage++;
            }

            // Process the desired page.
            if (groupsPage?.Value != null)
            {
                userGroups.AddRange(groupsPage.Value.Select(x => new GroupAccount
                {
                    Id = x.Id,
                    Name = x.DisplayName
                }));
            }

            return new PagedResponse<GroupAccount>
            {
                Items = userGroups,
                TotalItems = groupsPage?.OdataCount,
                HasNextPage = groupsPage?.OdataNextLink != null
            };
        }
    }
}
