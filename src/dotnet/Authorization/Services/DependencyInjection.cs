using FoundationaLLM.Authorization.Interfaces;
using FoundationaLLM.Authorization.Models.Configuration;
using FoundationaLLM.Authorization.Services;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    /// <summary>
    /// Authorization resource provider service implementation of resource provider dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Add the Agent resource provider and its related services the the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddAuthorizationCore(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(new BlobStorageServiceSettings
                    {
                        AuthenticationType = BlobStorageAuthenticationTypes.AzureIdentity,
                        AccountName = builder.Configuration[EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Storage_AccountName]
                    }),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Authorization
                };
            });

            builder.Services.AddSingleton<IAuthorizationCore, AuthorizationCore>(sp =>
                new AuthorizationCore(
                    Options.Create<AuthorizationCoreSettings>(new AuthorizationCoreSettings
                    {
                        InstanceIds = [.. builder.Configuration[EnvironmentVariables.FoundationaLLM_AuthorizationAPI_InstanceIds]!.Split(',')]
                    }),
                    sp.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Authorization),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp.GetRequiredService<ILogger<AuthorizationCore>>()));
            builder.Services.ActivateSingleton<IAuthorizationCore>();
        }
    }
}
