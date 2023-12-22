using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class StorageQueueRequestSourceService : IRequestSourceService
    {
        private readonly RequestSourceServiceSettings _settings;
        private readonly ILogger<StorageQueueRequestSourceService> _logger;

        private readonly QueueClient _queueClient;

        /// <inheritdoc/>
        public string SourceName => _settings.Name;

        public StorageQueueRequestSourceService(
            RequestSourceServiceSettings settings,
            ILogger<StorageQueueRequestSourceService> logger)
        {
            _settings = settings;
            _logger = logger;

            var queueServiceClient = new QueueServiceClient(_settings.ConnectionString);
            _queueClient = queueServiceClient.GetQueueClient(_settings.Name);
        }

        /// <inheritdoc/>
        public async Task<bool> HasRequests()
        {
            var message = await _queueClient.PeekMessageAsync();
            return message != null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<(VectorizationRequest Request, string PopReceipt)>> ReceiveRequests(int count)
        {
            var receivedMessages = await _queueClient.ReceiveMessagesAsync(count, TimeSpan.FromSeconds(_settings.VisibilityTimeoutSeconds));

            return receivedMessages.HasValue
                ? receivedMessages.Value.Select<QueueMessage, (VectorizationRequest, string)>(m => new
                (
                    JsonSerializer.Deserialize<VectorizationRequest>(m.Body.ToString())!,
                    m.PopReceipt
                )).ToList()
                : new List<(VectorizationRequest, string)>();
        }

        /// <inheritdoc/>
        public async Task DeleteRequest(string requestId, string popReceipt) =>
            await _queueClient.DeleteMessageAsync(requestId, popReceipt);

        /// <inheritdoc/>
        public async Task SubmitRequest(VectorizationRequest request)
        {
            var serializedMessage = JsonSerializer.Serialize(request);
            await _queueClient.SendMessageAsync(serializedMessage);
        }
    }
}
