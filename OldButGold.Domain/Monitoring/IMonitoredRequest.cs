namespace OldButGold.Domain.Monitoring
{
    internal interface IMonitoredRequest
    {
        void MonitorSucces(DomainMetrics metrics);
        void MonitorFailure(DomainMetrics metrics);

    }
}
