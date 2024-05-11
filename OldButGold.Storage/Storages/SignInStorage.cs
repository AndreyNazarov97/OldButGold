﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Storage.Entities;

namespace OldButGold.Storage.Storages
{
    internal class SignInStorage : ISignInStorage
    {
        private readonly IGuidFactory guidFactory;
        private readonly IMapper mapper;
        private readonly ForumDbContext dbContext;

        public SignInStorage(
            IGuidFactory guidFactory,
            IMapper mapper,
            ForumDbContext dbContext)
        {
            this.guidFactory = guidFactory;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

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
            return  dbContext.Users
                .Where(u => u.Login.Equals(login))
                .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}