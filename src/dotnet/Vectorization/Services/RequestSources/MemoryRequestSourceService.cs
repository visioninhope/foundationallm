    using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;
using System;
using FoundationaLLM.Vectorization.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using FoundationaLLM.Vectorization.Models.Configuration;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class MemoryRequestSourceService : IRequestSourceService
    {
        private readonly RequestSourceServiceSettings _settings;
        private readonly ILogger<MemoryRequestSourceService> _logger;
        private readonly ConcurrentQueue<VectorizationRequest> _requests = new ConcurrentQueue<VectorizationRequest>();

        /// <inheritdoc/>
        public string SourceName => _settings.Name;

        public MemoryRequestSourceService(
            RequestSourceServiceSettings settings,
            ILogger<MemoryRequestSourceService> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task<bool> HasRequests() =>
            Task.FromResult(_requests.Count > 0);

        /// <inheritdoc/>
        public Task<IEnumerable<(VectorizationRequest Request, string PopReceipt)>> ReceiveRequests(int count)
        {
            var result = new List<(VectorizationRequest, string)>();

            for (int i = 0; i < count; i++)
            {
                if (_requests.TryDequeue(out var request))
                    result.Add(new (request, string.Empty));
                else
                    break;
            }
            
            return Task.FromResult<IEnumerable<(VectorizationRequest, string)>>(result);
        }

        /// <inheritdoc/>
        public Task DeleteRequest(string requestId, string popReceipt) => Task.CompletedTask;

        /// <inheritdoc/>
        public Task SubmitRequest(VectorizationRequest request)
        {
            _requests.Enqueue(request);
            return Task.CompletedTask;
        }
    }
}
