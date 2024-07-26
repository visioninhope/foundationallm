namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all App Configuration key names used by FoundationaLLM.
    /// </summary>
    public static class AppConfigurationKeysNew
    {
        #region FoundationaLLM:Instance
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Instance:Id setting.
        /// <para>Value description:<br/>The unique identifier of the FoundationaLLM instance.</para>
        /// </summary>
        public const string FoundationaLLM_Instance_Id =
            "FoundationaLLM:Instance:Id";

        #endregion

        #region FoundationaLLM:Configuration
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:KeyVaultURL setting.
        /// <para>Value description:<br/>The URL of the Azure Key Vault providing the secrets.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_KeyVaultURL =
            "FoundationaLLM:Configuration:KeyVaultURL";

        #endregion

        #region FoundationaLLM:Configuration:ResourceProviderService
        
        /// <summary>
        /// The app configuration key for the FoundationaLLM:Configuration:ResourceProviderService:Storage:AuthenticationType setting.
        /// <para>Value description:<br/>The type of authentication used to connect to the storage used by the FoundationaLLM.Configuration resource provider.</para>
        /// </summary>
        public const string FoundationaLLM_Configuration_ResourceProviderService_Storage_AuthenticationType =
            "FoundationaLLM:Configuration:ResourceProviderService:Storage:AuthenticationType";

        #endregion
    }
}
