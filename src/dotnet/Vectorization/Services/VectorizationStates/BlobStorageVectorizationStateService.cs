using Azure.Core;
using Azure.Storage.Blobs;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    public class BlobStorageVectorizationStateService : IVectorizationStateService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        readonly VectorizationStateServiceSettings _settings;

        public BlobStorageVectorizationStateService(string containerName, IOptions<VectorizationStateServiceSettings> options)
        {
            _settings = options.Value;
            _blobServiceClient = new BlobServiceClient(_settings.BlobStorageConnection);
            _containerName = containerName;
        }

        /// <inheritdoc/>
        public async Task<bool> HasState(VectorizationRequest request)
        {
            var blobClient = GetBlobClient(request.ContentId);

            return await blobClient.ExistsAsync();
        }

        /// <inheritdoc/>
        public async Task<VectorizationState> ReadState(VectorizationRequest request)
        {
            var blobClient = GetBlobClient(request.ContentId);

            var response = await blobClient.DownloadAsync();
            using (var reader = new StreamReader(response.Value.Content))
            {
                var content = await reader.ReadToEndAsync();
                return JsonSerializer.Deserialize<VectorizationState>(content)!;
            }
        }

        /// <inheritdoc/>
        public async Task SaveState(VectorizationState state)
        {
            var blobClient = GetBlobClient(state.ContentId);

            var content = JsonSerializer.Serialize(state);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            await blobClient.UploadAsync(stream, true);
        }

        private BlobClient GetBlobClient(string contentId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            return containerClient.GetBlobClient(contentId);
        }
    }
}
