namespace FoundationaLLM.Vectorization.Models.Configuration
{
    /// <summary>
    /// Provides configuration settings to initialize a SharePoint Online content source service.
    /// </summary>
    public class SharePointOnlineContentSourceServiceSettings
    {
        /// <summary>
        /// The application (client) identifier of the Azure app registration.
        /// </summary>
        public required string ClientId { get; set; }

        /// <summary>
        /// The Azure tenant identifier where the app was registered.
        /// </summary>
        public required string TenantId { get; set; }

        /// <summary>
        /// The Azure KeyVault URL in which the X.509 certificate is stored.
        /// </summary>
        public required string KeyVaultURL { get; set; }

        /// <summary>
        /// The name of the X.509 certificate stored in Azure KeyVault.
        /// </summary>
        public required string CertificateName { get; set; }
    }
}
