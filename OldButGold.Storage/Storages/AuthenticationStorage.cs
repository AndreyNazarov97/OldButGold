using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.Authentication;

namespace OldButGold.Storage.Storages
{
    internal class AuthenticationStorage : IAuthenticationStorage
    {
        private readonly ForumDbContext dbContext;
        private readonly IMapper mapper;

        public AuthenticationStorage(
            ForumDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Session> FindSession(Guid sessionId, CancellationToken cancellationToken)
        {
            return await dbContext.Sessions
                .Where(s => s.SessionId == sessionId)
                .ProjectTo<Session>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken); 

        }
    }
}
