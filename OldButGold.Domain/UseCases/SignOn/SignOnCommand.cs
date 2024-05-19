using MediatR;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.SignOn
{
    public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>, IMonitoredRequest
    {
        private const string CounterName = "user.sign-on";
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
