using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Collections;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides group membership services.
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Retrieves the group identifiers list of the groups where the specified user principal is a member.
        /// </summary>
        /// <param name="userIdentifier">The user identifier for which group membership is retrieved. Can be either an object id or a user principal name (UPN).</param>
        /// <returns></returns>
        Task<List<string>> GetGroupsForPrincipalAsync(string userIdentifier);

        /// <summary>
        /// Retrieves a list of group accounts with filtering and paging options.
        /// </summary>
        /// <param name="queryParams">The filtering and paging options used when retrieving group accounts.</param>
        /// <returns></returns>
        Task<PagedResponse<GroupAccount>> GetUserGroupsAsync(AccountQueryParameters queryParams);

        /// <summary>
        /// Retrieves a group account by its identifier.
        /// </summary>
        /// <param name="groupId">The group account identifier used to retrieve a single group account.</param>
        /// <returns></returns>
        Task<GroupAccount> GetUserGroupByIdAsync(string groupId);
    }
}
