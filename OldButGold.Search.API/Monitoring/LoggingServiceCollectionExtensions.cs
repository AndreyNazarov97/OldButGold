using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;
using System.Diagnostics;

namespace OldButGold.Search.API.Monitoring
{
    public static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddApiLogging(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            return services.AddLogging(b => b
                .Configure(options => options.ActivityTrackingOptions =
                    ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId)
                .AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.WithProperty("Application", "OldButGold.Search.API")
                    .Enrich.WithProperty("Environment", environment.EnvironmentName)
                    .Enrich.With<TracingContextEnricher>()
                    .WriteTo.Logger(lc => lc
                        .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                        .WriteTo.GrafanaLoki(
                            configuration.GetConnectionString("Logs")!,
                            propertiesAsLabels: new[]
                            {
                                "level", "Environment", "Application", "SourceContext"
                            },
                            leavePropertiesIntact: true))
                .CreateLogger()));
        }
        private class TracingContextEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var activity = Activity.Current;
                if (activity is null) return;

                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                        "TraceId", activity.TraceId));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                        "SpanId", activity.SpanId));
            }
        }
    }


}
