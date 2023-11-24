using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Handlers
{
    public class PartitionHandler : IVectorizationStepHandler
    {
        public Task<(VectorizationRequest Request, VectorizationState State)> Invoke(VectorizationRequest request, VectorizationState state)
        {
            throw new NotImplementedException();
        }
    }
}
