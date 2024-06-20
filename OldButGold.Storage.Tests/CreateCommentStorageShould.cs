using FluentAssertions;
using OldButGold.Forums.Storage.Entities;
using OldButGold.Forums.Storage.Storages;
using Comment = OldButGold.Forums.Domain.Models.Comment;

namespace OldButGold.Forums.Storage.Tests
{
    public class CreateCommentStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
    {
        private readonly CreateCommentStorage sut = new(
            fixture.GetMapper(), fixture.GetDbContext(), new GuidFactory(), new MomentProvider());

        [Fact]
        public async Task ReturnNullForTopic_WhenNoMatchingTopic()
        {
            var topicId = Guid.Parse("a178233f-67ed-4f4b-8f6b-31ffa04bb3b0");

            var actual = await sut.FindTopic(topicId, CancellationToken.None);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task ReturnFoundTopic_WhenTopicIsPresentInDb()
        {
            var topicId = Guid.Parse("2bbabdc7-218a-4b4b-8127-62a7b78acd2e");
            var userId = Guid.Parse("86f1a38a-2d4e-4ca9-ba9f-3c0f1ccce070");
            var forumId = Guid.Parse("3cff7d06-83f0-4f65-bba9-031bc49e4f9e");

            await using var dbContext = fixture.GetDbContext();
            await dbContext.Topics.AddAsync(new Topic
            {
                TopicId = topicId,
                Author = new User
                {
                    UserId = userId,
                    Login = "Test user",
                    PasswordHash = [],
                    Salt = []
                },
                Forum = new Forum
                {
                    ForumId = forumId,
                    Title = "Test forum"
                },
                Title = "Test topic",
                CreatedAt = new DateTimeOffset(2024, 06, 20, 22, 41, 00, TimeSpan.Zero),
            }, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var actual = await sut.FindTopic(topicId, CancellationToken.None);
            actual.Should().BeEquivalentTo(new Domain.Models.Topic
            {
                Id = topicId,
                UserId = userId,
                ForumId = forumId,
                CreatedAt = new DateTimeOffset(2024, 06, 20, 22, 41, 00, TimeSpan.Zero),
                Title = "Test topic"
            });   
        }

        [Fact]
        public async Task ReturnNewlyCreatedComment_WhenCreating()
        {
            var topicId = Guid.Parse("b1febce0-1f8d-4986-9bac-6d6e6ff91bdd");
            var userId = Guid.Parse("ce018f5b-a94f-4fae-b6c0-cd5525c47ba3");
            var forumId = Guid.Parse("2e61733f-5818-4ef4-8c0b-e595f2fd14e2");
            var createdAt = new DateTimeOffset(2024, 06, 20, 22, 41, 00, TimeSpan.Zero);

            await using var dbContext = fixture.GetDbContext();
            await dbContext.Topics.AddAsync(new Topic
            {
                TopicId = topicId,
                Author = new User
                {
                    UserId = userId,
                    Login = "Test user",
                    PasswordHash = [],
                    Salt = []
                },
                Forum = new Forum
                {
                    ForumId = forumId,
                    Title = "Test forum"
                },
                Title = "Test topic",
                CreatedAt = createdAt,
            }, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var comment = await sut.CreateComment(topicId, userId, "Test comment", CancellationToken.None);
            comment.Should().BeEquivalentTo(new Comment
            {
                Text = "Test comment",
                TopicId = topicId,
                UserId = userId,
                AuthorLogin = "Test user"
            }, cfg => cfg.Excluding(c => c.Id).Excluding(c => c.CreatedAt));
        }
    }
}
