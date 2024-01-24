using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Management.Interfaces;
using FoundationaLLM.Management.Models.Configuration.Cache;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Management.Services.APIServices
{
    /// <summary>
    /// Contains base functionality for calling the DataSourceHubAPI service.
    /// </summary>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    /// <param name="logger">The logging interface used to log under the
    /// <see cref="DataSourceHubAPIService"/> type name.</param>
    public class DataSourceHubAPIService(
        IHttpClientFactoryService httpClientFactoryService,
        ILogger<DataSourceHubAPIService> logger) : APIServiceBase(
            Common.Constants.HttpClients.DataSourceHubAPI,
            httpClientFactoryService,
            logger), IDataSourceHubAPIService
    {
        
    }
}
