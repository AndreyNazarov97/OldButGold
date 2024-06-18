using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.GetTopics;
using OldButGold.Forums.Storage;

namespace OldButGold.Forums.Storage.Storages;

internal class GetTopicsStorage(
    ForumDbContext dbContext) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);
        var resources = await query
            .Select(t => new Topic
            {
                Id = t.TopicId,
                ForumId = t.ForumId,
                CreatedAt = t.CreatedAt,
                Title = t.Title,
                UserId = t.UserId,
            })
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}