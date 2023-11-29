using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;
using System;
using FoundationaLLM.Vectorization.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class MemoryRequestSourceService : RequestSourceServiceBase, IRequestSourceService
    {
        private readonly string _sourceName;
        private readonly ILogger<MemoryRequestSourceService> _logger;
        private readonly ConcurrentQueue<VectorizationRequest> _requests = new ConcurrentQueue<VectorizationRequest>();

        public MemoryRequestSourceService(
            string sourceName,
            ILogger<MemoryRequestSourceService> logger)
        {
            _sourceName = sourceName;
            _logger = logger;
        }

        public Task DeleteRequest(string requestId)
        {
            return Task.CompletedTask;
        }

        public Task<bool> HasRequests()
        {
            return Task.FromResult(
                _requests.Count > 0);
        }

        public Task<IEnumerable<VectorizationRequest>> ReceiveRequests(int count)
        {
            var result = new List<VectorizationRequest>();

            for (int i = 0; i < count; i++)
            {
                if (_requests.TryDequeue(out var request))
                    result.Add(request);
                else
                    break;
            }
            
            return Task.FromResult<IEnumerable<VectorizationRequest>>(result);
        }

        public Task SubmitRequest(VectorizationRequest request)
        {
            _requests.Enqueue(request);
            return Task.CompletedTask;
        }
    }
}
