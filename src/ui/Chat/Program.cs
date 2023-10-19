using Azure.Identity;
using FoundationaLLM.Chat.Helpers;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration;
using FoundationaLLM.Common.Models.Configuration.Authentication;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Polly;
using System;
using Azure.Monitor.OpenTelemetry.Exporter;

using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration["FoundationaLLM:AppConfig:ConnectionString"]);
    options.ConfigureKeyVault(options =>
    {
        options.SetCredential(new DefaultAzureCredential());
    });
});
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

builder.Services.AddHttpClient(FoundationaLLM.Common.Constants.HttpClients.CoreAPI,
        httpClient =>
        {
            httpClient.BaseAddress = new Uri(builder.Configuration["FoundationaLLM:APIs:CoreAPI:APIUrl"]);
        })
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(
            3, retryNumber => TimeSpan.FromMilliseconds(600)));

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeScopes = true;

    logging.AddConsoleExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "FoundationaLLM.Chat", serviceVersion: "0.0.1"));
    //.AddAzureMonitorLogExporter(o => o.ConnectionString = "InstrumentationKey=110912dc-f6eb-41c2-bc0b-2420492cc32e;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/");
});

builder.Services.AddOpenTelemetry().WithTracing(builder =>
{
    builder
    .AddHttpClientInstrumentation()
    //.AddConsoleExporter()
    .AddJaegerExporter()
    .AddSource("ChatManager")
    .AddSource("FoundationaLLM.Chat")
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "FoundationaLLM.Chat", serviceVersion: "0.0.1"));
});

// Setup Traces
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddConsoleExporter()
    .AddSource("FoundationaLLM.Chat")
    //.AddAzureMonitorTraceExporter(o => o.ConnectionString = "InstrumentationKey=110912dc-f6eb-41c2-bc0b-2420492cc32e;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/")
    .Build();

builder.Services.Configure<EntraSettings>(builder.Configuration.GetSection("FoundationaLLM:Chat:Entra"));
builder.Services.AddOptions<KeyVaultConfigurationServiceSettings>()
    .Bind(builder.Configuration.GetSection("FoundationaLLM:Configuration"));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientSecret = builder.Configuration["FoundationaLLM:Chat:Entra:ClientSecret"]; ;
        options.Instance = builder.Configuration["FoundationaLLM:Chat:Entra:Instance"];
        options.TenantId = builder.Configuration["FoundationaLLM:Chat:Entra:TenantId"];
        options.ClientId = builder.Configuration["FoundationaLLM:Chat:Entra:ClientId"];
        options.CallbackPath = builder.Configuration["FoundationaLLM:Chat:Entra:CallbackPath"];
    })
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { builder.Configuration["FoundationaLLM:Chat:Entra:Scopes"] })
    .AddInMemoryTokenCaches();
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.RegisterServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages(); // If Razor pages
    endpoints.MapControllers();
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();

static class ProgramExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IChatManager, ChatManager>();
        services.AddScoped<IBrandManager, BrandManager>();
        services.AddScoped<IBrandingService, BrandingService>();
        services.AddScoped<IAuthenticatedHttpClientFactory, EntraAuthenticatedHttpClientFactory>();
    }
}
