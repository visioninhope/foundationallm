using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IRequestManagerService
    {
        Task<VectorizationRequest> ReadCurrentRequest();

        Task CreateNextRequest();

        Task UpdateVectorizationState(string hash, VectorizationState state);
    }
}
