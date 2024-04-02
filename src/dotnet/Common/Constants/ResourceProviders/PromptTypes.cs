namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// Contains constants for the types of prompts.
    /// </summary>
    public static class PromptTypes
    {
        /// <summary>
        /// Basic agent without practical functionality. Used as base for all the other agents.
        /// </summary>
        public const string Basic = "basic";

        /// <summary>
        /// Multipart prompt composed of a prefix and a suffix.
        /// </summary>
        public const string Multipart = "multipart";
    }
}
