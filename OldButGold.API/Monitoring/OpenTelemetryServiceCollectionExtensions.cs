using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OldButGold.API.Monitoring
{
    internal static class OpenTelemetryServiceCollectionExtensions
    {
        public static IServiceCollection AddApiMetrics(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithMetrics(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("OldButGold.Domain")
                    .AddPrometheusExporter())
                .WithTracing(builder => builder
                    .ConfigureResource(r => r.AddService("OldButGold"))
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(cfg => cfg.SetDbStatementForText = true)
                    .AddSource("OldButGold.Domain")
                    .AddConsoleExporter()
                    .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));


            return services;
        }
    }
}
