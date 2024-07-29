using Asp.Versioning;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Middleware;
using FoundationaLLM.Common.Models.Configuration.API;
using FoundationaLLM.Common.Models.Configuration.Branding;
using FoundationaLLM.Common.Models.Context;
using FoundationaLLM.Common.OpenAPI;
using FoundationaLLM.Common.Services.Azure;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Common.Validation;
using FoundationaLLM.Management.Models.Configuration;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Services.RequestProcessors;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FoundationaLLM.Management.API
{
    /// <summary>
    /// Main entry point for the Management API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Management API service configuration.
        /// </summary>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DefaultAuthentication.Initialize(
                builder.Environment.IsProduction(),
                ServiceNames.ManagementAPI);

            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", false, true);
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddAzureAppConfiguration((Action<Microsoft.Extensions.Configuration.AzureAppConfiguration.AzureAppConfigurationOptions>)(options =>
            {
                options.Connect(builder.Configuration[EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString]);
                options.ConfigureKeyVault(options => { options.SetCredential(DefaultAuthentication.AzureCredential); });

                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Instance);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Configuration);

                options.Select(AppConfigurationKeyFilters.FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Branding);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_APIEndpoints_VectorizationAPI);

                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_Vectorization_Storage);  
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_Agent_Storage);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_Prompt_Storage);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_DataSource_Storage);                
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_Attachment_Storage);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_ResourceProviders_AIModel_Storage);

                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Events_Profiles_ManagementAPI);
            }));

            if (builder.Environment.IsDevelopment())
                builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

            builder.AddGroupMembership();
            builder.AddAuthorizationService();

            // CORS policies
            builder.AddCorsPolicies();

            builder.Services.AddOptions<VectorizationServiceSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_VectorizationAPI));
            builder.Services.AddOptions<CosmosDbSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_CoreAPI_Configuration_CosmosDB));
            builder.Services.AddOptions<ClientBrandingConfiguration>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Branding));
            builder.Services.AddOptions<AppConfigurationSettings>()
                .Configure(o =>
                    o.ConnectionString = builder.Configuration[EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString]!);

            builder.Services.AddInstanceProperties(builder.Configuration);

            // Add Azure ARM services
            builder.Services.AddAzureResourceManager();

            // Add event services
            builder.Services.AddAzureEventGridEvents(
                builder.Configuration,
                AppConfigurationKeySections.FoundationaLLM_Events_Profiles_ManagementAPI);

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddScoped<ICallContext, CallContext>();
            builder.AddHttpClientFactoryService();

            // Resource validation.
            builder.Services.AddSingleton<IResourceValidatorFactory, ResourceValidatorFactory>();

            // Register the remote vectorization processor, for calls into the Vectorization API.            
            builder.Services.AddSingleton<IVectorizationRequestProcessor, RemoteVectorizationRequestProcessor>();

            //----------------------------
            // Resource providers
            //----------------------------
            builder.AddAuthorizationResourceProvider();
            builder.AddConfigurationResourceProvider();
            builder.AddVectorizationResourceProvider();
            builder.AddAgentResourceProvider();
            builder.AddPromptResourceProvider();
            builder.AddDataSourceResourceProvider();
            builder.AddAttachmentResourceProvider();
            builder.AddAIModelResourceProvider();

            // Add authentication configuration.
            var e2ETestEnvironmentValue = Environment.GetEnvironmentVariable(EnvironmentVariables.FoundationaLLM_Environment) ?? string.Empty;
            var isE2ETestEnvironment = e2ETestEnvironmentValue.Equals(EnvironmentTypes.E2ETest, StringComparison.CurrentCultureIgnoreCase);
            builder.AddAuthenticationConfiguration(
                AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_Instance,
                AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_TenantId,
                AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_ClientId,
                AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_Scopes,
                requireScopes: !isE2ETestEnvironment,
                allowACLAuthorization: isE2ETestEnvironment
            );

            // Add OpenTelemetry.
            builder.AddOpenTelemetry(
                AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_AppInsightsConnectionString,
                ServiceNames.ManagementAPI);

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services
                .AddApiVersioning(options =>
                {
                    // Reporting api versions will return the headers
                    // "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(new DateOnly(2024, 2, 16));
                })
                .AddMvc()
                .AddApiExplorer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(
                options =>
                {
                    // Add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
                    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                    // Integrate xml comments
                    options.IncludeXmlComments(filePath);

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "azure_auth",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new[] {"user_impersonation"}
                        }
                    });

                    options.AddSecurityDefinition("azure_auth", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Azure Active Directory Oauth2 Flow",
                        Name = "azure_auth",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri("https://login.microsoftonline.com/common/oauth2/authorize"),
                                Scopes = new Dictionary<string, string>
                                {
                                    {
                                        "user_impersonation",
                                        "impersonate your user account"
                                    }
                                }
                            }
                        },
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                })
                .AddSwaggerGenNewtonsoftSupport();

            var app = builder.Build();

            // Set the CORS policy before other middleware.
            app.UseCors(CorsPolicyNames.AllowAllOrigins);

            // For the CoreAPI, we need to make sure that UseAuthentication is called before the UserIdentityMiddleware.
            app.UseAuthentication();
            app.UseAuthorization();

            // Register the middleware to extract the user identity context and other HTTP request context data required by the downstream services.
            app.UseMiddleware<CallContextMiddleware>();

            app.UseExceptionHandler(exceptionHandlerApp
                => exceptionHandlerApp.Run(async context
                    => await Results.Problem().ExecuteAsync(context)));

            // Configure the HTTP request pipeline.
            app.UseSwagger(p => p.SerializeAsV2 = true);
            app.UseSwaggerUI(
                options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }

                    options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>() { { "resource", builder.Configuration[AppConfigurationKeys.FoundationaLLM_APIEndpoints_ManagementAPI_Configuration_Entra_ClientId]! } });
                });

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
