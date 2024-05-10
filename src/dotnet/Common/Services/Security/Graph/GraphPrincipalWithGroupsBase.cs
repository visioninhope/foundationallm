using Microsoft.Graph;

namespace FoundationaLLM.Common.Services.Security.Graph
{
    public abstract class GraphPrincipalWithGroupsBase
    {
        protected readonly GraphServiceClient _graphServiceClient;
        public GraphPrincipalWithGroupsBase(GraphServiceClient graphServiceClient) => _graphServiceClient = graphServiceClient;
    }
}
