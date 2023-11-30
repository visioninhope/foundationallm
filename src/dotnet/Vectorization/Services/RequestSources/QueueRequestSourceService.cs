using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class QueueRequestSourceService : IRequestSourceService
    {
        public QueueRequestSourceService()
        {
        }

        public Task DeleteRequest(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRequests()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VectorizationRequest>> ReceiveRequests(int count)
        {
            throw new NotImplementedException();
        }

        public Task SubmitRequest(VectorizationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
