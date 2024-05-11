using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services.Security.Graph;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace FoundationaLLM.Common.Services.Security
{
    /// <summary>
    /// Implements group membership services using the Microsoft Graph API.
    /// </summary>
    public class MicrosoftGraphGroupMembershipService : IGroupMembershipService
    {
        private readonly GraphServiceClient _graphClient = new GraphServiceClient(
            DefaultAuthentication.AzureCredential);
        private readonly ILogger<MicrosoftGraphGroupMembershipService> _logger = LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger<MicrosoftGraphGroupMembershipService>();
        private readonly IGraphPrincipalWithGroups[] _graphPrincipalsWithGroups;

        public MicrosoftGraphGroupMembershipService() =>
            _graphPrincipalsWithGroups = [
                new ServicePrincipals(_graphClient),
                new Users(_graphClient)
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
                    _logger.LogError(error, "Error getting group membership for {UserIdentifier}", userIdentifier);
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
