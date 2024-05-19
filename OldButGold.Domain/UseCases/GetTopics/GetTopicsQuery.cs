using MediatR;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.GetTopics
{
    public record GetTopicsQuery(Guid ForumId, int Skip, int Take)
        : IRequest<(IEnumerable<Topic> resources, int totalCount)>, IMonitoredRequest
    {
        private const string CounterName = "topics.fetched";
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
