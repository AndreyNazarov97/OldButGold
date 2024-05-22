using AutoMapper;
using OldButGold.Forums.Domain.DomainEvents;
using OldButGold.Forums.Storage.Entities;
using System.Text.Json;
using ForumDomainEvent = OldButGold.Forums.Storage.Models.ForumDomainEvent;

namespace OldButGold.Forums.Storage.Storages
{
    internal class DomainEventStorage(
        ForumDbContext dbContext,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider,
        IMapper mapper) : IDomainEventStorage
    {
        public async Task AddEvent(Domain.DomainEvents.ForumDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var storageDomainEvent = mapper.Map<ForumDomainEvent>(domainEvent);

            await dbContext.DomainEvents.AddAsync(new DomainEvent
            {
                DomainEventId = guidFactory.Create(),
                EmittedAt = momentProvider.Now,
                ContentBlob = JsonSerializer.SerializeToUtf8Bytes(storageDomainEvent),
            }, cancellationToken);
            await dbContext.SaveChangesAsync();
        }
    }
}
