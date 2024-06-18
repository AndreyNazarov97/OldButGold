using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.UseCases.SignIn;
using OldButGold.Forums.Storage.Entities;

namespace OldButGold.Forums.Storage.Storages;

internal class SignInStorage(
    IGuidFactory guidFactory,
    IMapper mapper,
    ForumDbContext dbContext) : ISignInStorage
{
    public async Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken)
    {
        var sessionId = guidFactory.Create();

        await dbContext.Sessions.AddAsync(new Session
        {
            SessionId = sessionId,
            UserId = userId,
            ExpiresAt = expirationMoment,

        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);


        return sessionId;
    }

    public Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken)
    {
        return dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}