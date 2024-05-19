using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.UseCases.SignOut;

namespace OldButGold.Forums.Domain.Tests.SignOut
{
    public class SignOutUseCaseShould
    {
        private readonly ISetup<IIdentityProvider, IIdentity> currentIdentitySetup;
        private readonly Mock<ISignOutStorage> storage;
        private readonly ISetup<ISignOutStorage, Task> removeSessionSetup;
        private readonly ISetup<IIntentionManager, bool> signOutIsAllowedSetup;
        private readonly SignOutUseCase sut;

        public SignOutUseCaseShould()
        {
            var identityProvider = new Mock<IIdentityProvider>();
            currentIdentitySetup = identityProvider.Setup(p => p.Current);

            storage = new Mock<ISignOutStorage>();
            removeSessionSetup = storage.Setup(s => s.RemoveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

            var intentonManager = new Mock<IIntentionManager>();
            signOutIsAllowedSetup = intentonManager.Setup(m => m.IsAllowed(It.IsAny<AccountIntention>()));

            sut = new SignOutUseCase(
                intentonManager.Object,
                identityProvider.Object,
                storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenUserIsNotAuthenticated()
        {
            signOutIsAllowedSetup.Returns(false);

            await sut.Invoking(s => s.Handle(new SignOutCommand(), CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
        }


        [Fact]
        public async Task RemoveCurrentIdentitySesseion()
        {
            var sessionId = Guid.Parse("cd7bb4a7-84c9-4b29-b6d6-23007e38eebb");
            currentIdentitySetup.Returns(new User(Guid.Empty, sessionId));
            signOutIsAllowedSetup.Returns(true);
            removeSessionSetup.Returns(Task.CompletedTask);

            await sut.Handle(new SignOutCommand(), CancellationToken.None);

            storage.Verify(s => s.RemoveSession(sessionId, It.IsAny<CancellationToken>()), Times.Once);
            storage.VerifyNoOtherCalls();
        }
    }
}
