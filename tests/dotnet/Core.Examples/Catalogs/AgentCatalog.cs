using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Models.Agents;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Core.Examples.Constants;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    /// <summary>
    /// Contains the agent definitions for use in the FoundationaLLM Core examples.
    /// These definitions are used to create the agents in the FoundationaLLM Core examples.
    /// </summary>
    public static class AgentCatalog
    {
        #region Knowledge Management agents
        public static readonly List<KnowledgeManagementAgent> KnowledgeManagementAgents =
        [
            new KnowledgeManagementAgent()
            {
                Name = Agents.GenericInlineContextAgentName,
                Description = "A generic agent that can handle inline context completions.",
                SessionsEnabled = true,
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new OrchestrationSettings
                {
                    Orchestrator = Orchestrators.LangChain,
                    EndpointConfiguration = new Dictionary<string, object>
                    {
                        { "endpoint", "FoundationaLLM:AzureOpenAI:API:Endpoint" },
                        { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" },
                        { "api_version", "FoundationaLLM:AzureOpenAI:API:Version" }
                    },
                    ModelParameters = new Dictionary<string, object>
                    {
                        { "temperature", 0 },
                        { "deployment_name", "completions" }
                    }
                }
            }
        ];
        #endregion
    }
}
