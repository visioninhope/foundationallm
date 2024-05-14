using Microsoft.Graph;

namespace FoundationaLLM.Common.Services.Security.Graph
{
    /// <summary>
    /// Abstract class for Microsoft Entra ID principals with queryable group assignments.
    /// Refer to <see cref="ServicePrincipalsService"/> and <see cref="UsersService"/> classes.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GraphPrincipalWithGroupsBase"/> class.
    /// </remarks>
    /// <param name="graphServiceClient">The GraphServiceClient to be used for API interactions.</param>
    public abstract class GraphPrincipalWithGroupsBase(GraphServiceClient graphServiceClient)
    {
        /// <summary>
        /// The GraphServiceClient used to interact with Microsoft Graph API.
        /// </summary>
        protected readonly GraphServiceClient _graphServiceClient = graphServiceClient;
    }
}
