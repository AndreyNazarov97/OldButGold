using OldButGold.Forums.Domain;

namespace OldButGold.Forums.Domain.UseCases
{
    public interface IDomainEventStorage : IStorage
    {
        Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken);
    }
}
