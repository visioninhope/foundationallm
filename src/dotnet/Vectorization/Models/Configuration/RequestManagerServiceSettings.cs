using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models.Configuration
{
    public record RequestManagerServiceSettings
    {
        public required string RequestSourceName;

        public required Type HandlerType;

        public int MaxHandlerInstances;
    }
}
