using Azure.Storage.Queues;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    /// <summary>
    /// Implements a request source that uses Azure storage queues.
    /// </summary>
    public class StorageQueueRequestSourceService : IRequestSourceService
    {
        private readonly RequestSourceServiceSettings _settings;
        private readonly ILogger<StorageQueueRequestSourceService> _logger;
        private readonly QueueClient _queueClient;

        /// <inheritdoc/>
        public string SourceName => _settings.Name;

        /// <summary>
        /// Creates a new instance of the request source.
        /// </summary>
        /// <param name="settings">The <see cref="RequestSourceServiceSettings"/> object providing the settings.</param>
        /// <param name="logger">The logger used for logging.</param>
        
        public StorageQueueRequestSourceService(
            RequestSourceServiceSettings settings,
            ILogger<StorageQueueRequestSourceService> logger
            )
        {
            _settings = settings;
            _logger = logger;

            var queueServiceClient = new QueueServiceClient(_settings.ConnectionString);
            _queueClient = queueServiceClient.GetQueueClient(_settings.Name);
           
        }

        /// <inheritdoc/>
        public async Task<bool> HasRequests()
        {
            var message = await _queueClient.PeekMessageAsync().ConfigureAwait(false);
            return message.Value != null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<(VectorizationRequest Request, string MessageId, string PopReceipt)>> ReceiveRequests(int count, IVectorizationStateService vectorizationStateService)
        {
            var receivedMessages = await _queueClient.ReceiveMessagesAsync(count, TimeSpan.FromSeconds(_settings.VisibilityTimeoutSeconds)).ConfigureAwait(false);

            var result = new List<(VectorizationRequest, string, string)>();

            if (receivedMessages.HasValue)
            {
                foreach (var m in receivedMessages.Value)
                {
                    
                    try
                    {
                        var vectorizationRequest = JsonSerializer.Deserialize<VectorizationRequest>(m.Body.ToString());
                        if(vectorizationRequest is not null)
                        {
                            if (m.DequeueCount > _settings.MaxNumberOfRetries)
                            {
                                _logger.LogWarning("Message with id {MessageId} has been retried {DequeueCount} times and will be deleted.", m.MessageId, m.DequeueCount);
                                var state = await vectorizationStateService.HasState(vectorizationRequest).ConfigureAwait(false)
                                    ? await vectorizationStateService.ReadState(vectorizationRequest).ConfigureAwait(false)
                                    : VectorizationState.FromRequest(vectorizationRequest);
                                
                                state.LogEntries.Add(new VectorizationLogEntry(vectorizationRequest.Id!, m.MessageId, vectorizationRequest.CurrentStep ?? "StorageQueueService", "ERROR: The message has been retried too many times and will be deleted."));                                
                                await _queueClient.DeleteMessageAsync(m.MessageId, m.PopReceipt).ConfigureAwait(false);
                                await vectorizationStateService.SaveState(state).ConfigureAwait(false);
                                continue;
                            }
                        }
                        
                        result.Add(new(
                            vectorizationRequest!,
                            m.MessageId,
                            m.PopReceipt));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Cannot deserialize message with id {MessageId}.", m.MessageId);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task DeleteRequest(string messageId, string popReceipt) =>
            await _queueClient.DeleteMessageAsync(messageId, popReceipt).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task SubmitRequest(VectorizationRequest request)
        {
            var serializedMessage = JsonSerializer.Serialize(request);
            await _queueClient.SendMessageAsync(serializedMessage).ConfigureAwait(false);
        }
    }
}
