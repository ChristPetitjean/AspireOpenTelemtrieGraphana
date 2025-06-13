using OpenTelemetry;
using Wolverine_Rabbit.Metrics;
using Wolverine;

namespace Wolverine_Rabbit;

internal static class MetricsExtension
{
    public static WebApplicationBuilder AddCustomMetrics(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
               .WithMetrics(metrics =>
                {
                    metrics.AddMeter(MetricsCatalog.CountGreetings.Meter.Name);
                });
        return builder;
    }
}