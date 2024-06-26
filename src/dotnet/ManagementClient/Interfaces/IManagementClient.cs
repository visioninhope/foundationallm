namespace FoundationaLLM.Client.Management.Interfaces
{
    /// <summary>
    /// Provides high-level methods to interact with the Management API.
    /// </summary>
    public interface IManagementClient
    {
        /// <summary>
        /// Contains methods to interact with Agent resources.
        /// </summary>
        IAgentManagementClient Agents { get; }
        /// <summary>
        /// Contains methods to interact with Attachment resources.
        /// </summary>
        IAttachmentManagementClient Attachments { get; }
        /// <summary>
        /// Contains methods to interact with Configuration resources.
        /// </summary>
        IConfigurationManagementClient Configuration { get; }
        /// <summary>
        /// Contains methods to interact with DataSource resources.
        /// </summary>
        IDataSourceManagementClient DataSources { get; }
        /// <summary>
        /// Contains methods to interact with Prompt resources.
        /// </summary>
        IPromptManagementClient Prompts { get; }
        /// <summary>
        /// Contains methods to interact with Vectorization resources.
        /// </summary>
        IVectorizationManagementClient Vectorization { get; }
    }
}
