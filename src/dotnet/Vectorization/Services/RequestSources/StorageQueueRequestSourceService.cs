using Azure.Storage.Queues;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class StorageQueueRequestSourceService : IRequestSourceService
    {
        private readonly string _sourceName;
        private readonly ILogger<StorageQueueRequestSourceService> _logger;
        private readonly QueueClient _queueClient;
        readonly RequestSourceServiceSettings _settings;

        /// <inheritdoc/>
        public string SourceName => _sourceName;

        public StorageQueueRequestSourceService(
            string sourceName,
            IOptions<RequestSourceServiceSettings> options,
            ILogger<StorageQueueRequestSourceService> logger)
        {
            _settings = options.Value;
            _sourceName = sourceName;
            _logger = logger;

            var queueServiceClient = new QueueServiceClient(_settings.StorageQueueConnection);
            _queueClient = queueServiceClient.GetQueueClient(_sourceName);
        }

        /// <inheritdoc/>
        public async Task DeleteRequest(string requestId, string popReceipt) => await _queueClient.DeleteMessageAsync(requestId, popReceipt);

        /// <inheritdoc/>
        public async Task<bool> HasRequests()
        {
            var message = await _queueClient.PeekMessageAsync();
            return message != null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VectorizationRequest>> ReceiveRequests(int count)
        {
            var receivedMessages = await _queueClient.ReceiveMessagesAsync(count, TimeSpan.FromSeconds(_settings.VisibilityTimeout));
            return receivedMessages.Value.Select(message => JsonSerializer.Deserialize<VectorizationRequest>(message.Body.ToString())!).ToList();
        }

        /// <inheritdoc/>
        public async Task SubmitRequest(VectorizationRequest request)
        {
            var serializedMessage = JsonSerializer.Serialize(request);
            await _queueClient.SendMessageAsync(serializedMessage);
        }
    }
}
