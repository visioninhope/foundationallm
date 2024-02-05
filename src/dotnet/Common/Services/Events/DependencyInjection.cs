using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Events;
using FoundationaLLM.Common.Models.Events;
using FoundationaLLM.Common.Services.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoundationaLLM
{
    /// <summary>
    /// Provides extension methods used to configure dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Register the dependencies required to support Azure Event Grid events.
        /// </summary>
        /// <param name="services">Application builder service collection.</param>
        /// <param name="configuration">The <see cref="IConfigurationManager"/> providing access to configuration.</param>
        public static void AddAzureEventGridEvents(this IServiceCollection services,
            IConfigurationManager configuration)
        {
            services.AddOptions<AzureEventGridEventServiceSettings>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Events_AzureEventGridEventService));

            services.AddOptions<AzureEventGridEventServiceProfile>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Events_AzureEventGridEventService_Profiles_CoreAPI));

            services.AddSingleton<IEventService, AzureEventGridEventService>();
        }
    }
}
