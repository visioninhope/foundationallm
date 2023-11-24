using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStepHandler
    {
        Task<(VectorizationRequest Request, VectorizationState State)> Invoke(VectorizationRequest request, VectorizationState state);
    }
}
