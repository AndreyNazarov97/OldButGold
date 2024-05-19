using OldButGold.Search.Domain.Models;
using OldButGold.Search.Domain.UseCases.Index;
using OpenSearch.Client;
using SearchEntity = OldButGold.Search.Storage.Entities.SearchEntity;

namespace OldButGold.Search.Storage.Storages
{
    internal class IndexStorage(IOpenSearchClient client) : IIndexStorage
    {
        public async Task Index(Guid entityId, SearchEntityType entityType, string? title, string? text, CancellationToken cancellationToken)
        {
            await client.IndexAsync(new SearchEntity
            {
                EntityId = entityId,
                EntityType = (int)entityType,
                Title = title,
                Text = text,        
            }, descriptor =>
                descriptor.Index("obg-search-v1"), cancellationToken);
        }
    }
}
