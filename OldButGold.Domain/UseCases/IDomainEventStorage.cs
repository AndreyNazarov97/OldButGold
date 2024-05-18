namespace OldButGold.Domain.UseCases
{
    public interface IDomainEventStorage : IStorage
    {
        Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken);
    }
}
