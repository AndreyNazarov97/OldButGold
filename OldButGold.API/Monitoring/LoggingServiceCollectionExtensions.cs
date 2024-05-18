using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;
using System.Diagnostics;

namespace OldButGold.API.Monitoring
{
    public static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddApiLogging(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            var loggingLevelSwitch = new LoggingLevelSwitch();
            services.AddSingleton(loggingLevelSwitch);


            loggingLevelSwitch.MinimumLevelChanged += (sender, args) =>
            {
                Console.WriteLine($"Log level was switched from {args.OldLevel} to {args.NewLevel}");
            };

            return services.AddLogging(b => b
                .Configure(options => options.ActivityTrackingOptions =
                    ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId)
                .AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(loggingLevelSwitch)
                    .Enrich.WithProperty("Application", "OldButGold.API")
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
                if(activity is null)  return;

                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                        "TraceId", activity.TraceId));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                        "SpanId", activity.SpanId));
            }
        }
    }

    
}
