using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IRequestSourceService
    {
        Task<bool> HasRequests();

        Task<VectorizationRequest> ReceiveRequest();

        Task DeleteRequest(string requestId);

        Task SubmitRequest(VectorizationRequest request);
    }
}
