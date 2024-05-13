using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Monitoring;

namespace OldButGold.Domain.UseCases.SignIn
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
