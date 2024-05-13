using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Interfaces
{
    public interface IGraphPrincipalWithGroups
    {
        /// <summary>
        /// Query group assignments for the provided principal (object) ID.
        /// </summary>
        /// <param name="principalId">The <see cref="string"/> containing the principal ID.</param>
        /// <returns></returns>
        Task<List<Group>> GetGroups(string principalId);
    }
}
