﻿using FoundationaLLM.AgentFactory.Core.Models.Messages;
using FoundationaLLM.AgentFactory.Core.Models.Orchestration.Metadata;
using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.AgentFactory.Core.Interfaces;

/// <summary>
/// Interface for the AgentHub Service.
/// </summary>
public interface IAgentHubAPIService : IHubAPIService
{
    /// <summary>
    /// Gets a set of agents from the Agent Hub based on the prompt and user context.
    /// </summary>
    /// <param name="userPrompt">The user prompt to resolve.</param>
    /// <param name="sessionId">The session ID.</param>
    /// <returns></returns>
    Task<AgentHubResponse> ResolveRequest(string userPrompt, string sessionId);

    /// <summary>
    /// Gets the list with all the agent names and descriptions.
    /// </summary>
    /// <returns>A list of <see cref="MetadataBase"/> objects containing the names and descriptions of the agents.</returns>
    Task<List<MetadataBase>> ListAgents();
}
