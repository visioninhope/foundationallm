using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStepHandler
    {
        string StepId { get; }

        Task Invoke(VectorizationRequest request, VectorizationState state, CancellationToken cancellationToken);
    }
}
