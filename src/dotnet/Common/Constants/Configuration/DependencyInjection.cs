using FoundationaLLM.Common.Constants.Configuration;
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
    /// Dependency injection extensions for resource provider storage services.
    /// </summary>
    public static partial class DependencyInjection
    {        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.AIModel resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddAIModelResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_AIModel)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_AIModel_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_AIModel);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_AIModel
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.Agent resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddAgentResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Agent)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_Agent_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Agent);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Agent
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.Attachment resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddAttachmentResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_Attachment_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Attachment
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.Configuration resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddConfigurationResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Configuration)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_Configuration_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Configuration);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Configuration
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.DataSource resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddDataSourceResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_DataSource)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_DataSource_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_DataSource);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_DataSource
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.Prompt resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddPromptResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Prompt)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_Prompt_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Prompt);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Prompt
                };
            });
        }
        
        /// <summary>
        /// Add the named <see cref="IStorageService"/> implementation for the FoundationaLLM.Vectorization resource provider.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddVectorizationResourceProviderStorage(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Vectorization)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_ResourceProviders_Vectorization_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Vectorization);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Vectorization
                };
            });
        }
    }
}
