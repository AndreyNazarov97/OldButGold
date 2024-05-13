using MediatR;
using OldButGold.Domain.Models;
using OldButGold.Domain.Monitoring;

namespace OldButGold.Domain.UseCases.GetForums
{
    public record GetForumsQuery() : IRequest<IEnumerable<Forum>>, IMonitoredRequest
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
