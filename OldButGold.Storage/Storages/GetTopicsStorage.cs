using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.UseCases.GetTopics;

namespace OldButGold.Storage.Storages
{
    internal class GetTopicsStorage : IGetTopicsStorage
    {
        private readonly ForumDbContext dbContext;

        public GetTopicsStorage(
            ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<(IEnumerable<Domain.Models.Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken)
        {
            var query = dbContext.Topics.Where(t => t.ForumId == forumId);

            var totalCount = await query.CountAsync(cancellationToken);
            var resources = await query
                .Select(t => new Domain.Models.Topic
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
}
