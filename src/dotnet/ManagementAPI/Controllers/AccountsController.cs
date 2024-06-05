using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Management.API.Controllers
{
    /// <summary>
    /// Provides account retrieval methods.
    /// </summary>
    /// <param name="callContext">The call context containing user identity details.</param>
    /// <param name="groupService">The <see cref="IGroupService"/> used for retrieving group account information.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    [Authorize(Policy = "DefaultPolicy")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route($"instances/{{instanceId}}/accounts")]
    public class AccountsController(
        ICallContext callContext,
        IGroupService groupService,
        ILogger<AccountsController> logger) : Controller
    {
        private readonly ILogger<AccountsController> _logger = logger;
        private readonly ICallContext _callContext = callContext;

        /// <summary>
        /// Retrieves a list of group accounts with filtering and paging options.
        /// </summary>
        /// <returns></returns>
        [HttpGet("groups", Name = "GetGroups")]
        public async Task<IActionResult> GetGroups(AccountQueryParameters parameters)
        {
            var groups = await groupService.GetUserGroupsAsync(parameters);
            return new OkObjectResult(groups);
        }

        /// <summary>
        /// Retrieves a specific group account by its identifier.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("groups/{groupId}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(string groupId)
        {
            var group = await groupService.GetUserGroupByIdAsync(groupId);
            return new OkObjectResult(group);
        }
}
}
