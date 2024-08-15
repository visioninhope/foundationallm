using FluentValidation;
using FoundationaLLM.Attachment.ResourceProviders;
using FoundationaLLM.Attachment.Validation;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    /// <summary>
    /// Data Source resource provider service implementation of resource provider dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Add the Attachment resource provider and its related services the the dependency injection container.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddAttachmentResourceProvider(this IHostApplicationBuilder builder)
        {
            builder.AddAttachmentResourceProviderStorage();

            // Register validators.
            builder.Services.AddSingleton<IValidator<AttachmentFile>, AttachmentFileValidator>();

            builder.Services.AddSingleton<IResourceProviderService, AttachmentResourceProviderService>(sp =>
                new AttachmentResourceProviderService(
                    sp.GetRequiredService<IOptions<InstanceSettings>>(),
                    sp.GetRequiredService<IAuthorizationService>(),
                    sp.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment),
                    sp.GetRequiredService<IEventService>(),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp,
                    sp.GetRequiredService<ILoggerFactory>()));
            builder.Services.ActivateSingleton<IResourceProviderService>();
        }

        /// <summary>
        /// Add the Attachment resource provider and its related services the the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> dependency injection container service collection.</param>
        /// <param name="configuration">The <see cref="IConfigurationRoot"/> configuration manager.</param>
        public static void AddAttachmentResourceProvider(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddAttachmentResourceProviderStorage(configuration);

            // Register validators.
            services.AddSingleton<IValidator<AttachmentFile>, AttachmentFileValidator>();

            services.AddSingleton<IResourceProviderService, AttachmentResourceProviderService>(sp =>
                new AttachmentResourceProviderService(
                    sp.GetRequiredService<IOptions<InstanceSettings>>(),
                    sp.GetRequiredService<IAuthorizationService>(),
                    sp.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment),
                    sp.GetRequiredService<IEventService>(),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp,
                    sp.GetRequiredService<ILoggerFactory>()));
            services.ActivateSingleton<IResourceProviderService>();
        }
    }
}
