using Asp.Versioning;
using FoundationaLLM.Common.Models.Cache;
using FoundationaLLM.Common.Models.Configuration.Branding;
using FoundationaLLM.Management.Interfaces;
using FoundationaLLM.Management.Models.Configuration.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Management.API.Controllers
{
    /// <summary>
    /// Provides methods for interacting with the Configuration Management service.
    /// </summary>
    /// <remarks>
    /// Constructor for the Config Controller.
    /// </remarks>
    /// <param name="configurationManagementService">The Configuration Management service
    /// provides methods for managing configurations for FoundationaLLM.</param>
    /// <param name="cacheManagementService">Provides cache management methods for managing
    /// configuration and state of downstream services.</param>
    [Authorize]
    [Authorize(Policy = "RequiredScope")]
    [ApiVersion(1.0)]
    [ApiController]
    [Route("[controller]")]
    public class ConfigController(
        IConfigurationManagementService configurationManagementService,
        ICacheManagementService cacheManagementService) : ControllerBase
    {
        /// <summary>
        /// Returns the branding configuration from app configuration.
        /// </summary>
        [HttpGet("branding", Name = "GetBrandingConfigurations")]
        public async Task<ClientBrandingConfiguration> GetBrandingConfigurations() =>
            await configurationManagementService.GetBrandingConfigurationAsync();

        /// <summary>
        /// Updates the branding configuration in app configuration.
        /// </summary>
        /// <param name="brandingConfiguration"></param>
        /// <returns></returns>
        [HttpPut("branding", Name = "UpdateBrandingConfigurations")]
        public async Task UpdateBrandingConfigurations([FromBody] ClientBrandingConfiguration brandingConfiguration) =>
            await configurationManagementService.SetBrandingConfiguration(brandingConfiguration);

        /// <summary>
        /// Returns the configuration for global agent hints and feature setting.
        /// </summary>
        /// <returns></returns>
        [HttpGet("agents", Name = "GetAgentHints")]
        public async Task<AgentHints> GetAgentHints()
        {
            var agentHints = await configurationManagementService.GetAgentHintsAsync();
            var agentHintsEnabled = await configurationManagementService.GetAllowAgentSelectionAsync();
            return new AgentHints
            {
                Enabled = agentHintsEnabled,
                AllowedAgentSelection = agentHints
            };
        }

        /// <summary>
        /// Updates the configuration for global agent hints and feature setting.
        /// </summary>
        /// <param name="agentHints"></param>
        /// <returns></returns>
        [HttpPut("agents", Name = "UpdateAgentHints")]
        public async Task UpdateAgentHints([FromBody] AgentHints agentHints)
        {
            await configurationManagementService.UpdateAgentHintsAsync(agentHints.AllowedAgentSelection);
            await configurationManagementService.SetAllowAgentSelectionAsync(agentHints.Enabled);
        }

        /// <summary>
        /// Clears the agent cache from the relevant downstream services.
        /// </summary>
        /// <returns></returns>
        [HttpPost("cache/agent/clear", Name = "ClearAgentCache")]
        public async Task<APICacheRefreshResult> ClearAgentCache()
        {
            var result = await cacheManagementService.ClearAgentCache();
            return new APICacheRefreshResult
            {
                Success = result,
                Detail = result ? "Successfully cleared agent cache." : "Failed to clear agent cache."
            };
        }

        /// <summary>
        /// Clears the datasource cache from the relevant downstream services.
        /// </summary>
        /// <returns></returns>
        [HttpPost("cache/datasource/clear", Name = "ClearDataSourceCache")]
        public async Task<APICacheRefreshResult> ClearDataSourceCache()
        {
            var result = await cacheManagementService.ClearDataSourceCache();
            return new APICacheRefreshResult
            {
                Success = result,
                Detail = result ? "Successfully cleared datasource cache." : "Failed to clear datasource cache."
            };
        }

        /// <summary>
        /// Clears the prompt cache from the relevant downstream services.
        /// </summary>
        /// <returns></returns>
        [HttpPost("cache/prompt/clear", Name = "ClearPromptCache")]
        public async Task<APICacheRefreshResult> ClearPromptCache()
        {
            var result = await cacheManagementService.ClearPromptCache();
            return new APICacheRefreshResult
            {
                Success = result,
                Detail = result ? "Successfully cleared prompt cache." : "Failed to clear prompt cache."
            };
        }
    }
}
