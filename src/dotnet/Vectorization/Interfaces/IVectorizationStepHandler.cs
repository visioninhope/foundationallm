using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStepHandler
    {
        Task<VectorizationState> Invoke(VectorizationRequest request, VectorizationState state, CancellationToken cancellationToken);
    }
}
