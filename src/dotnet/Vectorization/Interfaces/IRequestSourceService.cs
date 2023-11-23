using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IRequestSourceService
    {
        Task<VectorizationRequest> ReadRequest();

        Task CreateRequest(VectorizationRequest request);
    }
}
