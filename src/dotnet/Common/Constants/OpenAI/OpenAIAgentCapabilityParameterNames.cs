using FoundationaLLM.Common.Interfaces;

namespace FoundationaLLM.Common.Constants.OpenAI
{
    /// <summary>
    /// Provides the names of the parameters that can be used to create OpenAI agent capabilities.
    /// </summary>
    /// <remarks>
    /// The constants are used by the callers of the <see cref="IGatewayServiceClient"/> implementations.
    /// </remarks>
    public static class OpenAIAgentCapabilityParameterNames
    {
        /// <summary>
        /// Indicates the need to create a new OpenAI assistant.
        /// </summary>
        public const string CreateAssistant = "OpenAI.Assistants.CreateAssistant";

        /// <summary>
        /// Indicates the need to create a new OpenAI assistant thread.
        /// </summary>
        public const string CreateAssistantThread = "OpenAI.Assistants.CreateAssistantThread";

        /// <summary>
        /// Provides the identifier of an existing OpenAI assistant.
        /// </summary>
        public const string AssistantId = "OpenAI.Assistants.AssistantId";

        /// <summary>
        /// Provides the prompt used by the OpenAI assistant.
        /// </summary>
        public const string AssistantPrompt = "OpenAI.Assistants.AssistantPrompt";

        /// <summary>
        /// Provides the identifier of an existing OpenAI assistant thread.
        /// </summary>
        public const string AssistantThreadId = "OpenAI.Assistants.AssistantThreadId";

        /// <summary>
        /// Provides the Azure OpenAI endpoint used to manage Open AI assistants.
        /// </summary>
        public const string Endpoint = "OpenAI.Assistants.Endpoint";

        /// <summary>
        /// Provides the model deployment name used by the OpenAI assistant.
        /// </summary>
        public const string ModelDeploymentName = "OpenAI.Assistants.ModelDeploymentName";
    }
}
