namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all configuration section names used to map configuration values to settings classes.
    /// </summary>
    public static partial class AppConfigurationKeySections
    {        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.AIModel resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_AIModel_Storage =
            "FoundationaLLM:ResourceProviders:AIModel:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Agent resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Agent_Storage =
            "FoundationaLLM:ResourceProviders:Agent:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Attachment resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Attachment_Storage =
            "FoundationaLLM:ResourceProviders:Attachment:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Configuration resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Configuration_Storage =
            "FoundationaLLM:ResourceProviders:Configuration:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.DataSource resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_DataSource_Storage =
            "FoundationaLLM:ResourceProviders:DataSource:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Prompt resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Prompt_Storage =
            "FoundationaLLM:ResourceProviders:Prompt:Storage";
        
        /// <summary>
        /// Configuration section used to identify the storage settings for the FoundationaLLM.Vectorization resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Vectorization_Storage =
            "FoundationaLLM:ResourceProviders:Vectorization:Storage";
    }
}
