using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStateService
    {
        Task<VectorizationState> ReadState(string id);

        Task UpdateState(VectorizationState state);
    }
}
