using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OldButGold.Search.ForumConsumer.Monitoring
{
    internal static class MetricsServiceCollectionExtensions
    {
        public static IServiceCollection AddApiMetrics(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithMetrics(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter())
                .WithTracing(builder => builder
                    .ConfigureResource(r => r.AddService("OldButGold.Search.ForumConsumer"))
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter += context => 
                            !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                            !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                        options.EnrichWithHttpResponse = (activity, response) =>
                            activity.AddTag("error", response.StatusCode >= 400);
                    })
                    .AddSource(Metrics.ApplicationName)
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!))); ;


            return services;
        }
    }
}
