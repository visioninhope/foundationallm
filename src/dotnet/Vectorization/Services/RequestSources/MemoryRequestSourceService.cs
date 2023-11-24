using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;
using System;
using FoundationaLLM.Vectorization.Interfaces;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class MemoryRequestSourceService : RequestSourceServiceBase, IRequestSourceService
    {
        public MemoryRequestSourceService()
        {
        }

        public async Task CreateRequest(VectorizationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRequests()
        {
            throw new NotImplementedException();
        }

        public async Task<VectorizationRequest> ReadRequest()
        {
            throw new NotImplementedException();
        }
    }
}
