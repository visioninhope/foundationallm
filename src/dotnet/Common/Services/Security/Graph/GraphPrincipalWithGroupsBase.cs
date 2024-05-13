using Microsoft.Graph;

namespace FoundationaLLM.Common.Services.Security.Graph
{
    /// <summary>
    /// Abstract class for Microsoft Entra principals with queryable group assignments.
    /// Refer to <see cref="ServicePrincipals"/> and <see cref="Users"/> classes.
    /// </summary>
    public abstract class GraphPrincipalWithGroupsBase
    {
        protected readonly GraphServiceClient _graphServiceClient;
        public GraphPrincipalWithGroupsBase(GraphServiceClient graphServiceClient) => _graphServiceClient = graphServiceClient;
    }
}
