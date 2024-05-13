using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Monitoring;

namespace OldButGold.Domain.UseCases.SignOn
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
