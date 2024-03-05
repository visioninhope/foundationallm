using FoundationaLLM;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

DefaultAuthentication.Production = builder.Environment.IsProduction();

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddEnvironmentVariables();
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

// Add services to the container.

// CORS policies
builder.AddCorsPolicies();

// Add authentication configuration.
builder.AddAuthenticationConfiguration(
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_Instance,
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_TenantId,
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_ClientId,
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_ClientSecret,
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_Entra_Scopes);

// Add OpenTelemetry.
builder.AddOpenTelemetry(
    EnvironmentVariables.FoundationaLLM_AuthorizationAPI_AppInsightsConnectionString,
    ServiceNames.AuthorizationAPI);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Set the CORS policy before other middleware.
app.UseCors(CorsPolicyNames.AllowAllOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem().ExecuteAsync(context)));

app.MapControllers();

app.Run();
