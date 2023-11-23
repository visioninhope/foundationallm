using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IVectorizationStepHandler
    {
        Task Invoke(string index, string step);
    }
}
