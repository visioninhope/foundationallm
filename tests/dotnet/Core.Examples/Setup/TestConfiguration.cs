using Azure.Data.AppConfiguration;
using FoundationaLLM.Core.Examples.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using Azure.Core;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.AzureAI;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FoundationaLLM.Core.Examples.Setup
{
    public sealed class TestConfiguration
    {
        private readonly IConfigurationRoot _configRoot;
        private static TestConfiguration? _instance;
        private static ConfigurationClient client;
        private readonly ChainedTokenCredential tokenCredential;
        public static CosmosDbSettings CosmosDbSettings;
        public static AgentPromptConfiguration AgentPromptConfiguration => LoadSection<AgentPromptConfiguration>();
        public static AzureAISettings AzureAISettings => LoadSection<AzureAISettings>();

		private TestConfiguration(IConfigurationRoot configRoot)
        {
            _configRoot = configRoot;

            tokenCredential = new(
                new AzureCliCredential(),
                new DefaultAzureCredential());
        }

        public static void Initialize(IConfigurationRoot configRoot, IServiceCollection services)
        {
            _instance = new TestConfiguration(configRoot);

            var connectionString =
                _instance._configRoot.GetValue<string>(EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString);
            client = new ConfigurationClient(connectionString);
            CosmosDbSettings = GetAppConfigSectionAsync<CosmosDbSettings>(AppConfigurationKeyFilters.FoundationaLLM_CosmosDB).GetAwaiter().GetResult();
        }

        public static TokenCredential GetTokenCredential()
        {
	        ValidateInstance();

	        return _instance!.tokenCredential;
        }

        private static T LoadSection<T>([CallerMemberName] string? caller = null)
        {
			ValidateInstance();

			if (string.IsNullOrEmpty(caller))
            {
                throw new ArgumentNullException(nameof(caller));
            }

            return _instance!._configRoot.GetSection(caller).Get<T>() ??
                   throw new ConfigurationNotFoundException(section: caller);
        }

        public static async Task<string> GetAppConfigValueAsync(string key)
        {
			ValidateInstance();

			var response = await client.GetConfigurationSettingAsync(key);

            if (response.Value is SecretReferenceConfigurationSetting secretReference)
            {
                var identifier = new KeyVaultSecretIdentifier(secretReference.SecretId);
                var secretClient = new SecretClient(identifier.VaultUri, _instance!.tokenCredential);
                var secret = await secretClient.GetSecretAsync(identifier.Name, identifier.Version);

                return secret.Value.Value;
            }

            return response.Value.Value;
        }

        public static async Task<T> GetAppConfigSectionAsync<T>(string keyFilter)
        {
			ValidateInstance();

			var selector = new SettingSelector { KeyFilter = keyFilter };

            T instance = Activator.CreateInstance<T>();

            await foreach (var setting in client.GetConfigurationSettingsAsync(selector))
            {
                string value;
                if (setting is SecretReferenceConfigurationSetting secretReference)
                {
                    var identifier = new KeyVaultSecretIdentifier(secretReference.SecretId);
                    var secretClient = new SecretClient(identifier.VaultUri, _instance!.tokenCredential);
                    var secret = await secretClient.GetSecretAsync(identifier.Name, identifier.Version);

                    value = secret.Value.Value;
                }
                else
                {
                    value = setting.Value;
                }

                var propertyKey = setting.Key.Split(':').Last();

                var property = typeof(T).GetProperty(propertyKey);
                if (property != null && property.CanWrite)
                {
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(instance, convertedValue);
                }
            }

            return instance;
        }

        private static void ValidateInstance()
        {
	        if (_instance == null)
	        {
		        throw new InvalidOperationException(
			        "TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
	        }
        }

	}
}
