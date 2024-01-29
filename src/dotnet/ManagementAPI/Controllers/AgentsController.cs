using System.Reflection.Metadata.Ecma335;
using Asp.Versioning;
using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Cache;
using FoundationaLLM.Common.Models.Configuration.Branding;
using FoundationaLLM.Management.Interfaces;
using FoundationaLLM.Management.Models.Configuration.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Management.API.Controllers
{
    /// <summary>
    /// Provides methods for interacting with the Agent resource provider.
    /// </summary>
    /// <remarks>
    /// Constructor for the Configurations Controller.
    /// </remarks>
    /// <param name="configurationManagementService">The Configuration Management service
    /// provides methods for managing configurations for FoundationaLLM.</param>
    [Authorize]
    [Authorize(Policy = "RequiredScope")]
    [ApiVersion(1.0)]
    [ApiController]
    [Route($"instances/{{instanceId}}/providers/{ResourceProviderNames.FoundationaLLM_Agent}/agents")]
    public class AgentsController(
        IConfigurationManagementService configurationManagementService) : ControllerBase
    {
        /// <summary>
        /// Returns the list of agents from the agent resource provider.
        /// </summary>
        [HttpGet(Name = "GetAgents")]
        public async Task<List<AgentBase>> GetAgents() =>
            new List<AgentBase>();

        /// <summary>
        /// Creates a new agent.
        /// </summary>
        /// <param name="agent">The agent to create.</param>
        /// <returns></returns>
        [HttpPost(Name = "CreateAgent")]
        public async Task CreateAgent([FromBody] AgentBase agent) =>
            Ok("Agent created.");

        /// <summary>
        /// Updates an agent.
        /// </summary>
        /// <param name="agent">The agent to update.</param>
        /// <returns></returns>
        [HttpPut(Name = "UpdateAgent")]
        public async Task UpdateAgent([FromBody] AgentBase agent) =>
            Ok("Agent updated.");

        /// <summary>
        /// Deletes an agent.
        /// </summary>
        /// <param name="agent">The agent to delete.</param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteAgent")]
        public async Task DeleteAgent([FromBody] AgentBase agent) =>
            Ok("Agent deleted.");
    }
}
