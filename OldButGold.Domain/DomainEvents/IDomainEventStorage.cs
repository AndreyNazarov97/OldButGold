namespace OldButGold.Forums.Domain.DomainEvents
{
    public interface IDomainEventStorage : IStorage
    {
        Task AddEvent(ForumDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
