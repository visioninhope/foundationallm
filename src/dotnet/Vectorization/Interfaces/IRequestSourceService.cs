using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IRequestSourceService
    {
        string SourceName { get; }

        Task<bool> HasRequests();

        Task<IEnumerable<VectorizationRequest>> ReceiveRequests(int count);

        Task DeleteRequest(string requestId);

        Task SubmitRequest(VectorizationRequest request);
    }
}
