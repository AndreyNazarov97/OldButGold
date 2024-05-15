using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace OldButGold.Domain.Monitoring
{
    internal abstract class MonitoringPipelineBehavior
    {
        protected static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
    }

    internal class MonitoringPipelineBehavior<TRequest, TResponse> : MonitoringPipelineBehavior, IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly DomainMetrics metrics;
        private readonly ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger;

        public MonitoringPipelineBehavior(
            DomainMetrics metrics,
            ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger)
        {
            this.metrics = metrics;
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();

            using var activity = DomainMetrics.ActivitySource.StartActivity(
                "usecase", ActivityKind.Internal ,default(ActivityContext));
            var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;

            activity?.AddTag("obg.command", request.GetType().Name);

            try
            {
                var result = await next.Invoke();
                monitoredRequest.MonitorSucces(metrics);
                return result;
            }
            catch(Exception ex)
            {
                monitoredRequest.MonitorFailure(metrics);
                logger.LogError(ex, "Unhandled error caught while hadling command {Command}", request);
                throw;
            }
        }
    }
}
