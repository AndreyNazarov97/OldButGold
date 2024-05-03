using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Storage;
using Forum = OldButGold.Storage.Forum;

namespace OldButGold.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly ForumDbContext forumDbContext;
        private readonly ISetup<IGuidFactory, Guid> createIdSetup;
        private readonly ISetup<IMomentProvider, DateTimeOffset> getNowSetup;
        private readonly CreateTopicUseCase sut;

        public CreateTopicUseCaseShould()
        {
            var dbContextoptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>().UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
            forumDbContext = new ForumDbContext(dbContextoptionsBuilder.Options);

            var guidFactory = new Mock<IGuidFactory>();
            createIdSetup =  guidFactory.Setup(f => f.Create());

            var momentProvider = new Mock<IMomentProvider>();
            getNowSetup = momentProvider.Setup(p => p.Now);

            sut = new CreateTopicUseCase(forumDbContext, guidFactory.Object, momentProvider.Object);
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMathcingForum()
        {
            await forumDbContext.Forums.AddAsync(new Forum
            {
                ForumId = Guid.Parse("d0964fcc-c7e6-450c-ad71-d53173f7e905"),
                Title = "Basic Forum",
            });
            await forumDbContext.SaveChangesAsync();

            var forumId = Guid.Parse("4eea5436-e706-464e-b146-8095569443ac");
            var authorId = Guid.Parse("0f493f76-dc27-40be-b3e9-48bdb33940e7");

            await sut.Invoking(s => s.Execute(forumId, "Some Title", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
        }


        [Fact]
        public async Task ReturnNewlyCreatedTopic()
        {
            var forumId = Guid.Parse("1a9a7075-e9d4-452e-a4eb-d945cc609802");
            var userId = Guid.Parse("0ad1b8c1-9510-4a29-a0be-63be72dacb67");
            var title = "Hello World";

            await forumDbContext.Forums.AddAsync(new Forum
            {
                ForumId = Guid.Parse("04c66c9f-f73e-4cf8-9fd5-d1520ce66d28"),
                Title = "Existing Forum",
            });
            await forumDbContext.Users.AddAsync(new User()
            {
                UserId = Guid.Parse("acb66720-0239-4145-86f5-bf6d8752d1c9"),
                Login = "Alex",
            });
            await forumDbContext.SaveChangesAsync();

            createIdSetup.Returns(Guid.Parse("ff5580da-9767-4495-80d4-1f5995778977"));
            getNowSetup.Returns(new DateTimeOffset(2024, 05, 03, 22, 16, 00, TimeSpan.FromHours(3)));

            var actual = await sut.Execute(forumId, title, userId, CancellationToken.None);

            var allTopics = await forumDbContext.Topics.ToArrayAsync();
            allTopics.Should().BeEquivalentTo(new[]
            {
                new Storage.Topic()
                {
                    ForumId = forumId,
                    UserId = userId,
                    Title = title,
                }
            }, cfg => cfg.Including(t => t.ForumId).Including(t => t.UserId).Including(t => t.Title));
            actual.Should().BeEquivalentTo(new Models.Topic()
            {
                Title = title,
                Author = "Alex",
                CreatedAt = new DateTimeOffset(2024, 05, 03, 22, 16, 00, TimeSpan.FromHours(3)),
                Id = Guid.Parse("ff5580da-9767-4495-80d4-1f5995778977")
            });
        }
    }
}