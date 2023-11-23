using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.RequestSources
{
    public class QueueRequestSourceService : RequestSourceServiceBase, IRequestSourceService
    {
        public QueueRequestSourceService()
        {
        }

        public async Task CreateRequest(VectorizationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<VectorizationRequest> ReadRequest()
        {
            throw new NotImplementedException();
        }
    }
}
