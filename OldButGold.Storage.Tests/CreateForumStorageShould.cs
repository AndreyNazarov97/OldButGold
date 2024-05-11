using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Storage.Storages;

namespace OldButGold.Storage.Tests
{
    public class CreateForumStorageShould : IClassFixture<StorageTestFixture>
    {
        private readonly CreateForumStorage sut;
        private readonly StorageTestFixture fixture;

        public CreateForumStorageShould(StorageTestFixture fixture)
        {
            this.fixture = fixture;
 
            sut = new CreateForumStorage(
                fixture.GetMemoryCache(),
                new GuidFactory(),
                fixture.GetDbContext(),
                fixture.GetMapper());
            
        }

        [Fact]
        public async Task InsertNewForumInDatbase()
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
