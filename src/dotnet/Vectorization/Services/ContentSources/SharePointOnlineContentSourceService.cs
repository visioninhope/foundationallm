using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;
using PnP.Framework;
using PnP.Framework.Modernization.Cache;
using System.Security.Cryptography.X509Certificates;

namespace FoundationaLLM.Vectorization.Services.ContentSources
{
    /// <summary>
    /// Implements a vectorization content source for content residing in SharePoint Online.
    /// </summary>
    public class SharePointOnlineContentSourceService : ContentSourceServiceBase, IContentSourceService
    {
        private readonly SharePointOnlineContentSourceServiceSettings _settings;
        private readonly ILogger<SharePointOnlineContentSourceService> _logger;

        /// <summary>
        /// Creates a new instance of the vectorization content source.
        /// </summary>
        public SharePointOnlineContentSourceService(
            SharePointOnlineContentSourceServiceSettings settings,
            ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _logger = loggerFactory.CreateLogger<SharePointOnlineContentSourceService>();
        }

        /// <inheritdoc/>
        public async Task<string> ExtractTextFromFileAsync(List<string> multipartId, CancellationToken cancellationToken)
        {
            ValidateMultipartId(multipartId, 4);

            var binaryContent = await GetDocumentBinaryContent(
                $"{multipartId[0]}/{multipartId[1]}",
                $"{multipartId[1]}/{multipartId[2]}/{multipartId[3]}",
                cancellationToken);

            return await ExtractTextFromFileAsync(multipartId[2], binaryContent);
        }

        /// <summary>
        /// Retrieves the binary content of the specified SharePoint Online document.
        /// </summary>
        /// <param name="documentLibrarySiteUrl">The url of the document library.</param>
        /// <param name="documentRelativeUrl">The server relative url of the document.</param>
        /// <param name="cancellationToken">The cancellation token that signals that operations should be cancelled.</param>
        /// <returns>An object representing the binary contents of the retrieved document.</returns>
        private async Task<BinaryData> GetDocumentBinaryContent(string documentLibrarySiteUrl, string documentRelativeUrl, CancellationToken cancellationToken)
        {
            X509Certificate2 certificate = await GetCertificate();

            var authManager = new AuthenticationManager(_settings.ClientId, certificate, _settings.TenantId);
            using (var cc = authManager!.GetContext(documentLibrarySiteUrl, cancellationToken))
            {
                var file = cc.Web.GetFileByServerRelativeUrl(documentRelativeUrl);
                var stream = file.OpenBinaryStream();
                var bytes = stream.ToByteArray();

                return new BinaryData(bytes);
            };
        }

        /// <summary>
        /// Retrieves a X.509 certificate from the specified Azure KeyVault.
        /// </summary>
        /// <returns>The X.509 certificate.</returns>
        private async Task<X509Certificate2> GetCertificate()
        {
            ValidateSettings();

            var certificateClient = new CertificateClient(new Uri(_settings.KeyVaultURL!), new DefaultAzureCredential());
            var certificateWithPolicy = await certificateClient.GetCertificateAsync(_settings.CertificateName);
            var certificateIdentifier = new KeyVaultSecretIdentifier(certificateWithPolicy.Value.SecretId);

            var secretClient = new SecretClient(new Uri(_settings.KeyVaultURL!), new DefaultAzureCredential());
            var secret = await secretClient.GetSecretAsync(certificateIdentifier.Name, certificateIdentifier.Version);
            var secretBytes = Convert.FromBase64String(secret.Value.Value);

            return new X509Certificate2(secretBytes);
        }

        private void ValidateSettings()
        {
            if (_settings == null)
                throw new VectorizationException("Missing configuration settings for the SharePointOnlineContentSourceService.");

            if (string.IsNullOrWhiteSpace(_settings.ClientId))
                throw new VectorizationException("Missing ClientId in the SharePointOnlineContentSourceService configuration settings.");

            if (string.IsNullOrWhiteSpace(_settings.TenantId))
                throw new VectorizationException("Missing TenantId in the SharePointOnlineContentSourceService configuration settings.");

            if (string.IsNullOrWhiteSpace(_settings.KeyVaultURL))
                throw new VectorizationException("Missing KeyVaultURL in the SharePointOnlineContentSourceService configuration settings.");

            if (string.IsNullOrWhiteSpace(_settings.CertificateName))
                throw new VectorizationException("Missing CertificateName in the SharePointOnlineContentSourceService configuration settings.");
        }
    }
}
