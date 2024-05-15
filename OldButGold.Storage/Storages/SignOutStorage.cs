using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.UseCases.SignOut;

namespace OldButGold.Storage.Storages
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
