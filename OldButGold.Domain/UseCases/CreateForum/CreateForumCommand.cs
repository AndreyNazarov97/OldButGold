using MediatR;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.CreateForum
{
    public record CreateForumCommand(string Title) : IRequest<Models.Forum>, IMonitoredRequest
    {
        private const string CounterName = "forums.created";
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
