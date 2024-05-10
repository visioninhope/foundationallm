using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Interfaces
{
    public interface IGraphPrincipalWithGroups
    {
        Task<List<Group>> GetGroups(string principalId);
    }
}
