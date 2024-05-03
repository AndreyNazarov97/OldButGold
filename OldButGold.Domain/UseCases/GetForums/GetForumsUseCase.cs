using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.Models;
using OldButGold.Storage;
using Forum = OldButGold.Domain.Models.Forum;

namespace OldButGold.Domain.UseCases.GetForums
{
    public class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly ForumDbContext forumDbContext;

        public GetForumsUseCase(
            ForumDbContext forumDbContext)
        {
            this.forumDbContext = forumDbContext;
        }

        public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
        {
            return await forumDbContext.Forums
                .Select(f => new Forum()
                {
                    Id = f.ForumId,
                    Title = f.Title,
                })
                .ToArrayAsync(cancellationToken);
        }
    }
}
