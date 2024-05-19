using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.UseCases.SignOut;
using OldButGold.Forums.Storage;

namespace OldButGold.Forums.Storage.Storages
{
    internal class SignOutStorage(
        ForumDbContext dbContext) : ISignOutStorage
    {
        public async Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
        {
            var session = await dbContext.Sessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        }
    }
}
