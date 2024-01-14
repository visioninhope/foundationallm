using FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.Metadata
{
    /// <summary>
    /// CXO data source metadata model.
    /// </summary>
    public class CXODataSource : SearchServiceDataSource
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonProperty("configuration")]
        public CXOConfiguration? Configuration { get; set; }
    }
}
