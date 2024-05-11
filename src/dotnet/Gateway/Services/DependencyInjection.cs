using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Gateway.Client;
using FoundationaLLM.Gateway.Instrumentation;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models.Configuration;
using FoundationaLLM.Gateway.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FoundationaLLM
{
    /// <summary>
    /// General purpose dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Adds the core Gateway service the the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddGatewayCore(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<GatewayCoreSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Gateway));

            builder.Services.AddSingleton<IGatewayCore, GatewayCore>();
            builder.Services.AddHostedService<GatewayWorker>();
        }

        /// <summary>
        /// Adds the Gateway API service to the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddGatewayService(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<GatewayServiceSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIs_GatewayAPI));

            builder.Services.AddScoped<IGatewayServiceClient, GatewayServiceClient>();
        }

        /// <summary>
        /// Add OpenTelemetry the the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        /// <param name="connectionStringConfigurationKey">The configuration key for the OpenTelemetry connection string.</param>
        /// <param name="serviceName">The name of the service.</param>
        public static void AddGatewayOpenTelemetry(this IHostApplicationBuilder builder,
            string connectionStringConfigurationKey,
            string serviceName)
        {
            // Add the OpenTelemetry telemetry service and send telemetry data to Azure Monitor.
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(serviceName: builder.Environment.ApplicationName))
                .WithTracing(builder =>
                {
                    builder
                    .AddSource(GatewayInstrumentation.ActivitySourceName)
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                    //builder.AddConsoleExporter();

                    builder.AddAzureMonitorTraceExporter();
                })
                .WithMetrics(builder =>
                {
                    builder
                    .AddMeter(GatewayInstrumentation.MeterName)
                    //.AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                    builder.AddAzureMonitorMetricExporter();

                })
                .UseAzureMonitor(options =>
                {
                    options.ConnectionString = builder.Configuration[connectionStringConfigurationKey];
                });

            // Create a dictionary of resource attributes.
            var resourceAttributes = new Dictionary<string, object> {
                { "service.name", serviceName },
                { "service.namespace", "FoundationaLLM" },
                { "service.instance.id", ValidatedEnvironment.MachineName }
            };

            // Configure the OpenTelemetry tracer provider to add the resource attributes to all traces.
            builder.Services.ConfigureOpenTelemetryTracerProvider((sp, builder) =>
                builder.ConfigureResource(resourceBuilder =>
                    resourceBuilder.AddAttributes(resourceAttributes)));
        }
    }
}
