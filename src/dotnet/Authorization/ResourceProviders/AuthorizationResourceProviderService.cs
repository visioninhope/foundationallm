using FoundationaLLM.Authorization.Interfaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Services.ResourceProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Authorization.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Authorization resource provider.
    /// </summary>
    /// <param name="instanceOptions"></param>
    /// <param name="downstreamAPIServices"></param>
    /// <param name="resourceValidatorFactory"></param>
    /// <param name="loggerFactory"></param>
    public class AuthorizationResourceProviderService(
        IOptions<InstanceSettings> instanceOptions,
        IAuthorizationAPIService authorizationAPIService,
        IResourceValidatorFactory resourceValidatorFactory,
        ILoggerFactory loggerFactory)
        : ResourceProviderServiceBase(
            instanceOptions.Value,
            null,
            null,
            resourceValidatorFactory,
            loggerFactory.CreateLogger<AuthorizationResourceProviderService>(),
            [])
    {
        private readonly IAuthorizationAPIService authorizationAPIService = authorizationAPIService;

        protected override Dictionary<string, ResourceTypeDescriptor> GetResourceTypes() =>
            AuthorizationResourceProviderMetadata.AllowedResourceTypes;

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Authorization;

        /// <inheritdoc/>
        protected override async Task InitializeInternal() =>
            await Task.CompletedTask;


    }
}
