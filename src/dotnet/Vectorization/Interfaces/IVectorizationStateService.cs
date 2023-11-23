using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStateService
    {
        Task<VectorizationState> ReadState(string hash);

        Task UpdateState(string hash, VectorizationState state);
    }
}
