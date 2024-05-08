using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.UseCases.SignIn;

namespace OldButGold.Storage.Storages
{
    internal class SignInStorage : IAuthenticationStorage
    {
        private readonly IMapper mapper;
        private readonly ForumDbContext dbContext;

        public SignInStorage(
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
