using Azure.Identity;
using FoundationaLLM.Vectorization.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration["FoundationaLLM:AppConfig:ConnectionString"]);

    options.ConfigureKeyVault(options =>
    {
        options.SetCredential(new DefaultAzureCredential());
    });
});

///TODO: builder.Services.AddHostedService<VectorizationWorker>();

builder.Services.AddApplicationInsightsTelemetryWorkerService(options =>
{
    options.ConnectionString = builder.Configuration["FoundationaLLM:VectorizationWorker:AppInsightsConnectionString"];
});

var host = builder.Build();

host.Run();