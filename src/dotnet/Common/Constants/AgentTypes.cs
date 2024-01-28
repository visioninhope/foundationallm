using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Constants
{
    /// <summary>
    /// Contains constants for the types of agents.
    /// </summary>
    public class AgentTypes
    {
        /// <summary>
        /// Knowledge Management agents are best for Q&A, summarization, and reasoning over textual data.
        /// </summary>
        public const string KnowledgeManagement = "knowledge-management";
        /// <summary>
        /// Analytic agents are best for querying, analyzing, calculating, and reporting on tabular data.
        /// </summary>
        public const string Analytic = "analytic";
    }
}
