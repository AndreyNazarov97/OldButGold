using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldButGold.Forums.Domain.UseCases.GetForums;

namespace OldButGold.Forums.Storage.Storages;

internal class GetForumStorage(
    IMemoryCache memoryCache,
    ForumDbContext dbContext,
    IMapper mapper) : IGetForumsStorage
{
    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return dbContext.Forums
                    .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
            });
    }
}