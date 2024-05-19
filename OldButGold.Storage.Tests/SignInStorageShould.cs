using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Storage.Entities;
using OldButGold.Forums.Storage.Storages;

namespace OldButGold.Forums.Storage.Tests
{
    public class SignInStorageFixture : StorageTestFixture
    {

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await using var dbContext = GetDbContext();
            await dbContext.Users.AddRangeAsync(new User
            {
                Login = "testUser",
                UserId = Guid.Parse("92b9d2f2-fd60-44a3-9f46-ef4d6ae7d04d"),
                Salt = new byte[] { 1 },
                PasswordHash = new byte[] { 2 },
            },
            new User
            {
                Login = "anotherUser",
                UserId = Guid.Parse("7aba9aba-8796-4322-87cb-fc2471963cd4"),
                Salt = new byte[] { 3 },
                PasswordHash = new byte[] { 4 },
            });
            await dbContext.SaveChangesAsync();
        }
    }

    public class SignInStorageShould(
        SignInStorageFixture fixture) : IClassFixture<SignInStorageFixture>
    {
        private readonly SignInStorage sut = new SignInStorage(new GuidFactory(), fixture.GetMapper(), fixture.GetDbContext());

        [Fact]
        public async Task ReturnUser_WhenDatabaseContainsUserWithSameLogin()
        {
            var actual = await sut.FindUser("testUser", CancellationToken.None);
            actual.Should().NotBeNull();
            actual!.UserId.Should().Be(Guid.Parse("92b9d2f2-fd60-44a3-9f46-ef4d6ae7d04d"));
        }

        [Fact]
        public async Task ReturnNull_WhenDatabaseDoesntContainsUserWithSameLogin()
        {
            var actual = await sut.FindUser("whatever", CancellationToken.None);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task ReturnNewlyCreatedSessionId()
        {
            var sessionId = await sut.CreateSession(
                Guid.Parse("92b9d2f2-fd60-44a3-9f46-ef4d6ae7d04d"),
                new DateTimeOffset(2024, 05, 10, 20, 14, 00, TimeSpan.Zero),
                CancellationToken.None);

            await using var dbContext = fixture.GetDbContext();
            (await dbContext.Sessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SessionId == sessionId)).Should().NotBeNull();

        }
    }
}
