using FluentValidation;
using FoundationaLLM.Authorization.Interfaces;
using FoundationaLLM.Authorization.Models;
using FoundationaLLM.Authorization.Models.Configuration;
using FoundationaLLM.Authorization.ResourceProviders;
using FoundationaLLM.Authorization.Services;
using FoundationaLLM.Authorization.Validation;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    /// <summary>
    /// Provides extension methods used to configure dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Register the FoundatiionaLLM.Authorization resource provider with the dependency injection container.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        public static void AddAuthorizationResourceProvider(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<AuthorizationAPIServiceSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIs_AuthorizationAPI));
            builder.Services.AddSingleton<IAuthorizationAPIService, AuthorizationAPIService>();

            // Register validators.
            builder.Services.AddSingleton<IValidator<ActionAuthorizationRequest>, ActionAuthorizationRequestValidator>();
            builder.Services.AddSingleton<IValidator<RoleAssignment>, RoleAssignmentValidator>();

            builder.Services.AddSingleton<IResourceProviderService, AuthorizationResourceProviderService>(sp =>
                new AuthorizationResourceProviderService(
                    sp.GetRequiredService<IOptions<InstanceSettings>>(),
                    sp.GetRequiredService<IAuthorizationAPIService>(),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp.GetRequiredService<ILoggerFactory>()));
            builder.Services.ActivateSingleton<IResourceProviderService>();
        }
    }
}
