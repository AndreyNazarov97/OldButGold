using OldButGold.Domain.UseCases;
using OldButGold.Storage.Entities;
using System.Text.Json;

namespace OldButGold.Storage.Storages
{
    internal class DomainEventStorage(
        ForumDbContext dbContext,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider) : IDomainEventStorage
    {
        public async Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.DomainEvents.AddAsync(new DomainEvent
            {
                DomainEventId = guidFactory.Create(),
                EmittedAt = momentProvider.Now,
                ContentBlob = JsonSerializer.SerializeToUtf8Bytes(entity),
            }, cancellationToken);
            await dbContext.SaveChangesAsync();
        }
    }
}
