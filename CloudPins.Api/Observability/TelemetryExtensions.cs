using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using CloudPins.Infrastructure.Observability;

namespace CloudPins.Api.Observability;

public static class TelemetryExtensions
{
    public static IServiceCollection AddCloudPinsTelemetry(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceName =
            configuration["OpenTelemetry:ServiceName"]
            ?? "cloudpins-api";

        var serviceVersion =
            configuration["OpenTelemetry:ServiceVersion"]
            ?? "1.0.0";

        var otlpEndpoint =
            configuration["OpenTelemetry:OtlpEndpoint"];

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: serviceName,
                    serviceVersion: serviceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment"] =
                        configuration["ASPNETCORE_ENVIRONMENT"]
                        ?? "Development"
                }))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag(
                                "http.request.user_agent",
                                request.Headers.UserAgent.ToString());
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource("CloudPins.Api")
                    .AddSource(CloudPinsTelemetry.ActivitySourceName);

                if (Uri.TryCreate(
                    otlpEndpoint,
                    UriKind.Absolute,
                    out var endpoint))
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpoint;
                    });
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter(CloudPinsTelemetry.MeterName);

                if (Uri.TryCreate(
                    otlpEndpoint,
                    UriKind.Absolute,
                    out var endpoint))
                {
                    metrics.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpoint;
                    });
                }
            });

        return services;
    }
}
