using System.Reflection.Metadata.Ecma335;
using System.Text;
using Asp.Versioning;
using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Agent.Models.Resources;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Cache;
using FoundationaLLM.Common.Models.Configuration.Branding;
using FoundationaLLM.Management.Interfaces;
using FoundationaLLM.Management.Models.Configuration.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

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
        public class PolymorphicAgentModelBinder : IModelBinder
        {
            public async Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var jsonString = await ReadJsonFromBody(bindingContext.HttpContext.Request);

                // Deserialize to AgentBase to get the type information
                var agentBase = JsonConvert.DeserializeObject<AgentBase>(jsonString);
                object agent;

                switch (agentBase.Type)
                {
                    case AgentTypes.KnowledgeManagement:
                        agent = JsonConvert.DeserializeObject<KnowledgeManagementAgent>(jsonString);
                        break;
                    // Handle other agent types.
                    default:
                        // Set a model state error if the type is unrecognized
                        bindingContext.ModelState.AddModelError("type", "Unknown agent type");
                        bindingContext.Result = ModelBindingResult.Failed();
                        return;
                }

                bindingContext.Result = ModelBindingResult.Success(agent);
                return;
            }

            private async Task<string> ReadJsonFromBody(HttpRequest request)
            {
                // Ensure the request body can be read multiple times
                if (!request.Body.CanSeek)
                {
                    request.EnableBuffering();
                }

                // Reset the request body to the beginning
                request.Body.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

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
        public async Task<IActionResult> CreateAgent([FromBody] AgentBase agent)
        {
            var newAgent = agent as KnowledgeManagementAgent;
            return Ok("Agent created.");
        }

        /// <summary>
        /// Updates an agent.
        /// </summary>
        /// <param name="agent">The agent to update.</param>
        /// <returns></returns>
        [HttpPut(Name = "UpdateAgent")]
        public async Task<IActionResult> UpdateAgent([FromBody] AgentBase agent) =>
            Ok("Agent updated.");

        /// <summary>
        /// Deletes an agent.
        /// </summary>
        /// <param name="agent">The agent to delete.</param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteAgent")]
        public async Task<IActionResult> DeleteAgent([FromBody] AgentBase agent) =>
            Ok("Agent deleted.");
    }
}
