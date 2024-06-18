using MediatR;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.CreateComment
{
    public record CreateCommentCommand(Guid TopicId, string Text) : IRequest<Models.Comment>, IMonitoredRequest
    {
        private const string CounterName = "comments.created";
        public void MonitorSucces(DomainMetrics metrics)
        {
            metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
        }
        public void MonitorFailure(DomainMetrics metrics)
        {
            metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));
        }
    }
}
