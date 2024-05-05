using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Storage.Storages
{
    internal class GetForumStorage : IGetForumsStorage
    {
        private readonly IMemoryCache memoryCache;
        private readonly ForumDbContext dbContext;

        public GetForumStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext)
        {
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
        {
            return await memoryCache.GetOrCreateAsync(
                nameof(GetForums),
                entry =>
                {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return dbContext.Forums
                    .Select(f => new Domain.Models.Forum()
                    {
                        Id = f.ForumId,
                        Title = f.Title,

                    })
                    .ToArrayAsync(cancellationToken);
                });  
        }
    }
}
