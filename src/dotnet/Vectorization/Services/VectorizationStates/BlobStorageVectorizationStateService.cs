using Azure.Storage.Blobs;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    /// <summary>
    /// Provides vectorization state persistence services using  Azure blob storage.
    /// </summary>
    public class BlobStorageVectorizationStateService : VectorizationStateServiceBase, IVectorizationStateService
    {
        private readonly BlobStorageService _storageService;
        private readonly BlobStorageVectorizationStateServiceSettings _settings;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Creates a new vectorization state service instance.
        /// </summary>
        /// <param name="options">The options used to configure the new instance.</param>
        /// <param name="loggerFactory">The logger factory used to create loggers.</param>
        public BlobStorageVectorizationStateService(
            IOptions<BlobStorageVectorizationStateServiceSettings> options,
            ILoggerFactory loggerFactory)
        {
            _settings = options.Value;
            _loggerFactory = loggerFactory;
            _storageService = new BlobStorageService(
                Options.Create(_settings.Storage),
                _loggerFactory.CreateLogger<BlobStorageService>());
        }

        /// <inheritdoc/>
        public async Task<bool> HasState(VectorizationRequest request) =>
            await _storageService.FileExistsAsync(
                _settings.StorageContainerName,
                GetPersistenceIdentifier(request.ContentIdentifier));

        /// <inheritdoc/>
        public async Task<VectorizationState> ReadState(VectorizationRequest request)
        {
            var content = await _storageService.ReadFileAsync(
                _settings.StorageContainerName,
                GetPersistenceIdentifier(request.ContentIdentifier));

            return JsonSerializer.Deserialize<VectorizationState>(content)!;
        }

        /// <inheritdoc/>
        public async Task SaveState(VectorizationState state)
        {
            var content = JsonSerializer.Serialize(state);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            await _storageService.WriteFileAsync(
                _settings.StorageContainerName,
                GetPersistenceIdentifier(state.ContentIdentifier),
                stream);
        }

        /// <inheritdoc/>
        protected override string GetPersistenceIdentifier(VectorizationContentIdentifier contentIdentifier) =>
            $"{contentIdentifier.CanonicalId}_state_{HashContentIdentifier(contentIdentifier)}.json";
    }
}
