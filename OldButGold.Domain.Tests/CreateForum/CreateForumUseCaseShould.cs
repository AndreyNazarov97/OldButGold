using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.UseCases.CreateForum;

namespace OldButGold.Forums.Domain.Tests.CreateForum
{
    public class CreateForumUseCaseShould
    {
        private readonly Mock<ICreateCommentStorage> storage;
        private readonly ISetup<ICreateCommentStorage, Task<Models.Forum>> createForumSetup;
        private readonly CreateForumUseCase sut;

        public CreateForumUseCaseShould()
        {
            var intentionManager = new Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.IsAllowed(It.IsAny<ForumIntention>()))
                .Returns(true);

            storage = new Mock<ICreateCommentStorage>();
            createForumSetup = storage.Setup(s => s.CreateForum(It.IsAny<string>(), It.IsAny<CancellationToken>()));

            sut = new CreateForumUseCase(intentionManager.Object, storage.Object);
        }

        [Fact]
        public async Task ReturnCreatedForum()
        {
            var forumTitle = "Hello";
            var forum = new Models.Forum()
            {
                Title = forumTitle,
                Id = Guid.Parse("d4f47c25-40ce-4580-9e7c-0cf566d3467d"),
            };
            createForumSetup.ReturnsAsync(forum);

            var actual = await sut.Handle(new CreateForumCommand(forumTitle), CancellationToken.None);
            actual.Should().BeEquivalentTo(forum);

            storage.Verify(s => s.CreateForum(forumTitle, It.IsAny<CancellationToken>()), Times.Once);
            storage.VerifyNoOtherCalls();
        }

    }
}
