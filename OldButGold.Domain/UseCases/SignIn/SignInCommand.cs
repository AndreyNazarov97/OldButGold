using MediatR;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Monitoring;

namespace OldButGold.Forums.Domain.UseCases.SignIn
{
    public record SignInCommand(string Login, string Password) : IRequest<(IIdentity identity, string token)>, IMonitoredRequest
    {
        private const string CounterName = "user.sign-in";
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
