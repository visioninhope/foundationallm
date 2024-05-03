using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FoundationaLLM.Core.Examples.Services
{
    /// <inheritdoc/>
    public class VectorizationAPITestManager(
        IHttpClientManager httpClientManager,
        IOptions<InstanceSettings> instanceSettings) : IVectorizationAPITestManager
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        
    }
}
