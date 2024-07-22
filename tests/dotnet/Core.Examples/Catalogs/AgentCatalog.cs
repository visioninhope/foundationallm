using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Models.ResourceProviders.AIModel;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
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
        /// <summary>
        /// Catalog of knowledge management agents.
        /// </summary>
        public static readonly List<KnowledgeManagementAgent> KnowledgeManagementAgents =
        [
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.GenericInlineContextAgentName,
                Description = "A generic agent that can handle inline context completions.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.LangChain,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.GenericInlineContextAgentName,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.SemanticKernelInlineContextAgentName,
                Description = "SemanticKernel agent that can handle inline context completions.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null,
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.SemanticKernelInlineContextAgentName,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.SemanticKernelAgentName,
                Description = "SemanticKernel agent that can handle completions.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null,
                    DataSourceObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.SemanticKernelAgentName,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.LangChainAgentName,
                Description = "LangChain agent that can handle completions.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null,
                    DataSourceObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.LangChain,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.LangChainAgentName,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.SemanticKernelSDZWA,
                Description = "Knowledge Management Agent that queries the San Diego Zoo Wildlife Alliance journals using SemanticKernel.",
                InlineContext = false,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.SemanticKernelSDZWA,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.LangChainSDZWA,
                Description = "Knowledge Management Agent that queries the San Diego Zoo Wildlife Alliance journals using LangChain.",
                InlineContext = false,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.LangChain,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.LangChainSDZWA,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.ConversationGeneratorAgent,
                Description = "An agent that creates conversations based on product descriptions.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.LangChain,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.ConversationGeneratorAgent,
                        DeploymentName = "completions-gpt-4-32k",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0.5 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.Dune01,
                Description = "Knowledge Management Agent that queries the Dune books using SemanticKernel.",
                InlineContext = false,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.Dune01,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.Dune02,
                Description = "Inline Context Agent that writes poems about Dune suitable for being used in wartime songs.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.Dune02,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.Dune03,
                Description = "Answers questions about Dune by asking for help from other agents.",
                InlineContext = true,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.SemanticKernel,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.Dune03,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },
            new KnowledgeManagementAgent
            {
                Name = TestAgentNames.LangChainDune,
                Description = "Knowledge Management Agent that queries the Dune books using LangChain.",
                InlineContext = false,
                SessionsEnabled = true,
                Vectorization = new AgentVectorizationSettings
                {
                    DedicatedPipeline = false,
                    IndexingProfileObjectIds = null,
                    TextEmbeddingProfileObjectId = null
                },
                ConversationHistory = new ConversationHistory
                {
                    Enabled = true,
                    MaxHistory = 10
                },
                Gatekeeper = new Gatekeeper
                {
                    UseSystemSetting = false
                },
                OrchestrationSettings = new LLMOrchestrationSettings
                {
                    Orchestrator = LLMOrchestrationServiceNames.LangChain,
                    AIModel = new AIModelBase
                    {
                        Name = TestAgentNames.LangChainDune,
                        DeploymentName = "completions",
                        Endpoint = new APIEndpointConfiguration {
                            Name = "Azure OpenAI",
                            Provider = "microsoft",
                            Url = "FoundationaLLM:AzureOpenAI:API:Endpoint",
                            AuthenticationType = AuthenticationTypes.APIKey,
                            APIVersion = "FoundationaLLM:AzureOpenAI:API:Version",
                            AuthenticationParameters = new Dictionary<string, object> {
                                { "api_key", "FoundationaLLM:AzureOpenAI:API:Key" }
                            },
                            Category = APIEndpointCategory.LLM,
                            RetryStrategyName = "ExponentialBackoff",
                            TimeoutSeconds = 60
                        },
                        ModelParameters = new Dictionary<string, object>
                        {
                            { "temperature", 0 },
                        }
                    }
                }
            },

        ];
        #endregion

        /// <summary>
        /// Retrieves all agents defined in the catalog.
        /// </summary>
        /// <returns></returns>
        public static List<AgentBase> GetAllAgents()
        {
            var agents = new List<AgentBase>();
            agents.AddRange(KnowledgeManagementAgents);

            return agents;
        }
    }
}
