using MediatR;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.CreateTopic
{
    public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<Topic>, IMonitoredRequest
    {
        private const string CounterName = "topics.created";
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
