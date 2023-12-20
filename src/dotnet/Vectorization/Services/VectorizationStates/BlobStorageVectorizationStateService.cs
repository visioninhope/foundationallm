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
        public async Task<VectorizationState> ReadState(string id)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(id);

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
            await Task.CompletedTask;

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(state.CurrentRequestId);

            var content = JsonSerializer.Serialize(state);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            await blobClient.UploadAsync(stream, true);
        }
    }
}
