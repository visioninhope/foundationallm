using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants.Agents
{
    /// <summary>
    /// Contains constants for orchestrator names.
    /// </summary>
    public static class Orchestrators
    {
        /// <summary>
        /// The LangChain orchestrator.
        /// </summary>
        public const string LangChain = "LangChain";
        /// <summary>
        /// The SemanticKernel orchestrator.
        /// </summary>
        public const string SemanticKernel = "SemanticKernel";
        /// <summary>
        /// The Azure OpenAI direct orchestrator.
        /// </summary>
        public const string AzureOpenAIDirect = "AzureOpenAIDirect";
        /// <summary>
        /// The Azure AI direct orchestrator.
        /// </summary>
        public const string AzureAIDirect = "AzureAIDirect";
    }
}
