using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models
{
    public class VectorizationRequest
    {
        public required string ContentId { get; set; }

        public required string ContentSourceType { get; set; }

        public required string ContentSourceName { get; set; }
    }
}
