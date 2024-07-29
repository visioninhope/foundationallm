using Asp.Versioning;
using FoundationaLLM;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.OpenAPI;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

DefaultAuthentication.Initialize(
    builder.Environment.IsProduction(),
    ServiceNames.GatewayAdapterAPI);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration[EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString]);
    options.ConfigureKeyVault(options =>
    {
        options.SetCredential(DefaultAuthentication.AzureCredential);
    });
    options.Select(AppConfigurationKeyFilters.FoundationaLLM_APIEndpoints_GatewayAdapterAPI);
});
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

builder.AddOpenTelemetry(
    AppConfigurationKeys.FoundationaLLM_APIEndpoints_GatewayAdapterAPI_AppInsightsConnectionString,
    ServiceNames.GatewayAdapterAPI);

// CORS policies
builder.AddCorsPolicies();

// Generic exception handling
builder.AddGatewayGenericExceptionHandling();

// Open API (Swagger)
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// API key validation
builder.Services.AddTransient<IAPIKeyValidationService, APIKeyValidationService>();

builder.Services.AddControllers();

// Add API Key Authorization
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<APIKeyAuthenticationFilter>();
builder.Services.AddOptions<APIKeyValidationSettings>()
    .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_GatewayAdapterAPI));

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
    })
    .AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Set the CORS policy before other middleware.
app.UseCors(CorsPolicyNames.AllowAllOrigins);

app.UseExceptionHandler();

app.UseSwagger();
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
    });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
