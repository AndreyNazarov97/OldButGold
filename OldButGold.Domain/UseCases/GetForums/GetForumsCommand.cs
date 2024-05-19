using MediatR;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.GetForums
{
    public record GetForumsQuery() : IRequest<IEnumerable<Models.Forum>>, IMonitoredRequest
    {
        private const string CounterName = "forums.fetched";
        public void MonitorFailure(DomainMetrics metrics)
        {
            metrics.IncrementCount(
                CounterName,
                1,
                DomainMetrics.ResultTags(false));
        }

        public void MonitorSucces(DomainMetrics metrics)
        {
            metrics.IncrementCount(
                CounterName,
                1,
                DomainMetrics.ResultTags(true));
        }
    }
}
