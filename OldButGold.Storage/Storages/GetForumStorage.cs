using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Storage.Storages
{
    internal class GetForumStorage : IGetForumsStorage
    {
        private readonly IMemoryCache memoryCache;
        private readonly ForumDbContext dbContext;
        private readonly IMapper mapper;

        public GetForumStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext,
            IMapper mapper)
        {
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
        {
            return await memoryCache.GetOrCreateAsync(
                nameof(GetForums),
                entry =>
                {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return dbContext.Forums
                    .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
                });  
        }
    }
}
