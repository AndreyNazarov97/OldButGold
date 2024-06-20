using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Storage;
using OldButGold.Forums.Storage.Storages;

namespace OldButGold.Forums.Storage.Tests
{
    public class CreateForumStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
    {
        private readonly CreateForumStorage sut = new(
                fixture.GetMemoryCache(),
                new GuidFactory(),
                fixture.GetDbContext(),
                fixture.GetMapper());

        [Fact]
        public async Task InsertNewForumInDatabase()
        {
            var forum = await sut.CreateForum("Test title", CancellationToken.None);
            forum.Id.Should().NotBeEmpty();

            await using var dbContext = fixture.GetDbContext();
            var forumTitles = await dbContext.Forums
                .Where(f => f.ForumId == forum.Id)
                .Select(f => f.Title).ToArrayAsync();

            forumTitles.Should().HaveCount(1).And.Contain("Test title");
        }

    }
}
