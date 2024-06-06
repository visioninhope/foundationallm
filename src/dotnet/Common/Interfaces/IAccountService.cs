using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Collections;
using Microsoft.Graph.Models;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides user and group account services.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves the group identifiers list of the groups where the specified user principal is a member.
        /// </summary>
        /// <param name="userIdentifier">The user identifier for which group membership is retrieved. Can be either an object id or a user principal name (UPN).</param>
        /// <returns></returns>
        Task<List<string>> GetGroupsForPrincipalAsync(string userIdentifier);

        /// <summary>
        /// Retrieves user and group objects by the passed in list of IDs.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<ObjectQueryResult>> GetObjectsByIdsAsync(ObjectQueryParameters parameters);

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

        /// <summary>
        /// Retrieves a list of user accounts with filtering and paging options.
        /// </summary>
        /// <param name="queryParams">The filtering and paging options used when retrieving users.</param>
        /// <returns></returns>
        Task<PagedResponse<UserAccount>> GetUsersAsync(AccountQueryParameters queryParams);

        /// <summary>
        /// Retrieves a user account by its identifier.
        /// </summary>
        /// <param name="groupId">The user identifier used to retrieve a single user account.</param>
        /// <returns></returns>
        Task<UserAccount> GetUserByIdAsync(string userId);
    }
}
