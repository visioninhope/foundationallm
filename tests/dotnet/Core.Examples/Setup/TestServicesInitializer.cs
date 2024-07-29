﻿using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.API;
using FoundationaLLM.Common.Models.Configuration.AzureAI;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Common.Services.API;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Exceptions;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using FoundationaLLM.Core.Examples.Services;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using FoundationaLLM.SemanticKernel.Core.Services.Indexing;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace FoundationaLLM.Core.Examples.Setup
{
    public static class TestServicesInitializer
	{
		/// <summary>
		/// Configure base services and dependencies for the tests.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configRoot"></param>
		public static void InitializeServices(
			IServiceCollection services,
			IConfigurationRoot configRoot)
		{
			TestConfiguration.Initialize(configRoot, services);

            services.AddOptions<BlobStorageServiceSettings>(
                    DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Vectorization)
                .Bind(configRoot.GetSection("FoundationaLLM:Vectorization:ResourceProviderService:Storage"));

            RegisterInstance(services, configRoot);
            RegisterClientLibraries(services, configRoot);
			RegisterCosmosDb(services, configRoot);
            RegisterAzureAIService(services, configRoot);
            RegisterLogging(services);
			RegisterServiceManagers(services);
            RegisterSearchIndex(services, configRoot);
        }

        private static void RegisterInstance(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<InstanceSettings>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Instance));
        }

		private static void RegisterSearchIndex(IServiceCollection services, IConfiguration configuration)
		{
            services.AddOptions<AzureAISearchIndexingServiceSettings>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_AzureAISearchVectorStore_Configuration));

            services.AddKeyedSingleton<IIndexingService, AzureAISearchIndexingService>(
                DependencyInjectionKeys.FoundationaLLM_APIEndpoints_AzureAISearchVectorStore_Configuration);

        }

        private static void RegisterClientLibraries(IServiceCollection services, IConfiguration configuration)
        {
            var instanceId = configuration.GetValue<string>(AppConfigurationKeys.FoundationaLLM_Instance_Id);
            services.AddCoreClient(
                configuration[AppConfigurationKeys.FoundationaLLM_APIEndpoints_CoreAPI_APIUrl]!,
                DefaultAuthentication.AzureCredential!);
            services.AddManagementClient(
                configuration[AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_APIUrl]!,
                DefaultAuthentication.AzureCredential!,
                instanceId);
        }

		private static void RegisterCosmosDb(IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions<CosmosDbSettings>()
				.Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB));

			services.AddSingleton<CosmosClient>(serviceProvider =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
				return new CosmosClientBuilder(settings.Endpoint, DefaultAuthentication.AzureCredential)
					.WithSerializerOptions(new CosmosSerializationOptions
					{
						PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
					})
					.WithConnectionModeGateway()
					.Build();
			});

			services.AddScoped<ICosmosDbService, CosmosDbService>();
		}

		private static void RegisterAzureAIService(IServiceCollection services, IConfiguration configuration)
		{
            try
            {
                var completionQualityMeasurementConfiguration = TestConfiguration.CompletionQualityMeasurementConfiguration;
                if (completionQualityMeasurementConfiguration is { AgentPrompts: not null })
                {
                    services.AddOptions<AzureAISettings>()
                        .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_AzureAIStudio_Configuration));
                    services.AddOptions<BlobStorageServiceSettings>()
                        .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_AzureAIStudio_Configuration_Storage));

                    services.AddScoped<IAzureAIService, AzureAIService>();
                    services.AddSingleton<IStorageService, BlobStorageService>();
                }
                else
                {
                    Console.WriteLine($"Skipping Azure AI Service initialization. No agent prompts defined in the {nameof(CompletionQualityMeasurementConfiguration)} configuration section.");
                }
            }
            catch (ConfigurationNotFoundException cex)
            {
                Console.WriteLine($"Skipping Azure AI Service initialization. {cex.Message}");
            }
		}

		private static void RegisterLogging(IServiceCollection services)
		{
			services.AddLogging(builder =>
			{
				builder.AddConsole();
			});
		}

        private static void RegisterServiceManagers(IServiceCollection services)
        {
            services.AddScoped<ICoreAPITestManager, CoreAPITestManager>();
			services.AddScoped<IManagementAPITestManager, ManagementAPITestManager>();
            services.AddScoped<IAuthenticationService, MicrosoftEntraIDAuthenticationService>();
            services.AddScoped<IHttpClientManager, HttpClientManager>();
			services.AddScoped<IAgentConversationTestService, AgentConversationTestService>();
            services.AddScoped<IVectorizationTestService, VectorizationTestService>();
        }
    }
}
