using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldButGold.Forums.Domain.UseCases.CreateForum;
using OldButGold.Forums.Storage.Entities;

namespace OldButGold.Forums.Storage.Storages;

internal class CreateForumStorage(
    IMemoryCache memoryCache,
    IGuidFactory guidFactory,
    ForumDbContext dbContext,
    IMapper mapper) : ICreateCommentStorage
{
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

        memoryCache.Remove(nameof(GetForumStorage.GetForums));

        return await dbContext.Forums
            .Where(f => f.ForumId == forumId)
            .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

}