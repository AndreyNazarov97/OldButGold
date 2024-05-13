﻿using MediatR;
using OldButGold.Domain.Monitoring;

namespace OldButGold.Domain.UseCases.SignOut
{
    public record SignOutCommand() : IRequest, IMonitoredRequest
    {
        private const string CounterName = "user.sign-out";
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
