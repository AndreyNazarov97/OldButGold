using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateForum;

namespace OldButGold.Domain.Tests.CreateForum
{
    public class CreateForumUseCaseShould
    {
        private readonly Mock<ICreateForumStorage> storage;
        private readonly ISetup<ICreateForumStorage, Task<Forum>> createForumSetup;
        private readonly CreateForumUseCase sut;

        public CreateForumUseCaseShould()
        {
            var validator = new Mock<IValidator<CreateForumCommand>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<CreateForumCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var intentionManager = new Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.IsAllowed(It.IsAny<ForumIntention>()))
                .Returns(true);

            storage = new Mock<ICreateForumStorage>();
            createForumSetup = storage.Setup(s => s.CreateForum(It.IsAny<string>(), It.IsAny<CancellationToken>()));

            sut = new CreateForumUseCase(validator.Object, intentionManager.Object, storage.Object);
        }

        [Fact]
        public async Task ReturnCreatedForum()
        {
            var forumTitle = "Hello";
            Forum forum = new Forum()
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
