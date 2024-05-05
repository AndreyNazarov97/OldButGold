using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Storage.Storages
{
    internal class GetForumStorage : IGetForumsStorage
    {
        private readonly ForumDbContext dbContext;

        public GetForumStorage(
            ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
        {
            return await dbContext.Forums
                .Select(f => new Domain.Models.Forum()
                {
                    Id = f.ForumId,
                    Title = f.Title,
                }).ToArrayAsync(cancellationToken);
        }
    }
}
