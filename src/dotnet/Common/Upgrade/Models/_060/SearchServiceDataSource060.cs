﻿using FoundationaLLM.Common.Upgrade.Models._040;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._060
{
    /// <summary>
    /// Search service data source metadata model.
    /// </summary>
    public class SearchServiceDataSource060 : DataSourceBase060
    {
        /// <summary>
        /// Search Service configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public SearchServiceConfiguration040? Configuration { get; set; }

    }
}
