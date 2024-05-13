using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.Tests.CreateTopic
{
    public class CreateTopicUseCaseShould
    {
        private readonly Mock<ICreateTopicStorage> storage;
        private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
        private readonly Mock<IGetForumsStorage> getForumsStorage;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Models.Forum>>> getForumsSetup;
        private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;
        private readonly CreateTopicUseCase sut;

        public CreateTopicUseCaseShould()
        {
            storage = new Mock<ICreateTopicStorage>();
            createTopicSetup = storage.Setup(s =>
                s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            getCurrentUserIdSetup = identity.Setup(s => s.UserId);

            intentionManager = new Mock<IIntentionManager>();
            intentionIsAllowedSetup = intentionManager.Setup(p => p.IsAllowed(It.IsAny<TopicIntention>()));

            var validator = new Mock<IValidator<CreateTopicCommand>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            sut = new CreateTopicUseCase(
                validator.Object, 
                intentionManager.Object, 
                identityProvider.Object, 
                getForumsStorage.Object , 
                storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("d85e2db1-02b8-4dc3-9bf3-e798dd22e03f");

            intentionIsAllowedSetup.Returns(false);

            await sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMathcingForum()
        {
            var forumId = Guid.Parse("4eea5436-e706-464e-b146-8095569443ac");

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(Array.Empty<Forum>());


            (await sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Some Title"), CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>())
                .Which.ErrorCode.Should().Be(DomainErrorCode.Gone);
        }


        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExist()
        {
            var forumId = Guid.Parse("1a9a7075-e9d4-452e-a4eb-d945cc609802");
            var userId = Guid.Parse("0ad1b8c1-9510-4a29-a0be-63be72dacb67");
            var title = "Hello World";

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(new Forum[] {new() { Id = forumId,  Title = title} } );
            getCurrentUserIdSetup.Returns(userId);
            var expected = new Topic();
            createTopicSetup.ReturnsAsync(expected);

            var actual = await sut.Handle(new CreateTopicCommand(forumId, title), CancellationToken.None);
            actual.Should().Be(expected);

            storage.Verify(s => s.CreateTopic(forumId, userId, title, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}