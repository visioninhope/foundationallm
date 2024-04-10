using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Core.Examples.Models
{
	/// <summary>
	/// Agent configurations used for agent completion samples.
	/// </summary>
	public class AgentPromptConfiguration
	{
		public AgentPrompt[] AgentPrompts { get; set; }
	}

	public class AgentPrompt
	{
		public string AgentName { get; set; }
		public SessionConfiguration SessionConfiguration { get; set; }
		public string UserPrompt { get; set; }
		public string ExpectedCompletion { get; set; }
	}

	public class SessionConfiguration
	{
		/// <summary>
		/// If true, the chat session will not be stored in the database and the session ID will be ignored.
		/// </summary>
		public bool Sessionless { get; set; }
		/// <summary>
		/// Create a new chat session rather than using an existing one.
		/// </summary>
		public bool CreateNewSession { get; set; }
		/// <summary>
		/// If you are not creating a new chat session, enter the existing session ID here.
		/// </summary>
		public string SessionId { get; set; }
	}

}
