using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.Authentication;

namespace OldButGold.Storage.Storages
{
    internal class AuthenticationStorage : IAuthenticationStorage
    {
        private readonly IMapper mapper;
        private readonly ForumDbContext dbContext;

        public AuthenticationStorage(
            IMapper mapper,
            ForumDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        public Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken)
        {
            return  dbContext.Users
                .Where(u => u.Login.Equals(login))
                .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
