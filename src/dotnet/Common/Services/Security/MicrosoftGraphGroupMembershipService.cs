using FoundationaLLM.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace FoundationaLLM.Common.Services.Security
{
    /// <summary>
    /// Implements group membership services using the Microsoft Graph API.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MicrosoftGraphGroupMembershipService"/> class.
    /// </remarks>
    public class MicrosoftGraphGroupMembershipService(
        ILogger<MicrosoftGraphGroupMembershipService> logger,
        IGraphPrincipalWithGroups servicePrincipalsService,
        IGraphPrincipalWithGroups usersService) : IGroupMembershipService
    {
        private readonly IGraphPrincipalWithGroups[] _graphPrincipalsWithGroups =
            [
                servicePrincipalsService,
                usersService
            ];

        /// <inheritdoc/>
        public async Task<List<string>> GetGroupsForPrincipal(string userIdentifier)
        {
            var groupMembership = new List<Group>();

            foreach (var graphPrincipalWithGroups in _graphPrincipalsWithGroups)
            {
                try
                {
                    groupMembership = await graphPrincipalWithGroups.GetGroups(userIdentifier);
                }
                catch (ODataError error)
                {
                    logger.LogError(error, "Error getting group membership for {UserIdentifier}", userIdentifier);
                    continue;
                }

                if (groupMembership.Count > 0) break;
            }

            return groupMembership.Count == 0
                ? []
                : groupMembership.Where(x => x.Id != null).Select(x => x.Id!).ToList();
        }
    }
}
