using MediatR;
using OldButGold.Domain.Models;
using OldButGold.Domain.Monitoring;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public record CreateForumCommand(string Title) : IRequest<Forum>, IMonitoredRequest
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
