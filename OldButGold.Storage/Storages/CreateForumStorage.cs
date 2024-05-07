using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Storage.Storages;

namespace OldButGold.Storage.DependencyIncjection
{
    internal class CreateForumStorage : ICreateForumStorage
    {
        private readonly IMemoryCache memoryCache;
        private readonly IGuidFactory guidFactory;
        private readonly ForumDbContext dbContext;

        public CreateForumStorage(
            IMemoryCache memoryCache,
            IGuidFactory guidFactory, 
            ForumDbContext dbContext)
        {
            this.memoryCache = memoryCache;
            this.guidFactory = guidFactory;
            this.dbContext = dbContext;
        }

        public async Task<Domain.Models.Forum> CreateForum(string title, CancellationToken cancellationToken)
        {
            var forumId = guidFactory.Create();

            var forum = new Forum()
            {
                ForumId = forumId,
                Title = title,
            };

            await dbContext.Forums.AddAsync(forum, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            memoryCache.Remove(nameof(GetForumStorage.GetForums)) ;    

            return await dbContext.Forums
                .Where(f => f.ForumId == forumId)
                .Select(f => new Domain.Models.Forum
                {
                    Id = f.ForumId,
                    Title = f.Title,
                })
                .FirstAsync(cancellationToken);
        }

    }
}