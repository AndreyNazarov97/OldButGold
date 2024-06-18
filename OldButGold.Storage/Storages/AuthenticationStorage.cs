using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Storage;

namespace OldButGold.Forums.Storage.Storages;

internal class AuthenticationStorage(
    ForumDbContext dbContext,
    IMapper mapper) : IAuthenticationStorage
{
    public async Task<Session> FindSession(Guid sessionId, CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .Where(s => s.SessionId == sessionId)
            .ProjectTo<Session>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    }
}