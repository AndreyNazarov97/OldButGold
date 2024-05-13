
using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;

namespace OldButGold.Domain.UseCases.SignOut
{
    internal class SignOutUseCase : IRequestHandler<SignOutCommand>
    {
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly ISignOutStorage storage;

        public SignOutUseCase(
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ISignOutStorage storage)
        {
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.storage = storage;
        }

        public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            intentionManager.ThrowIfForbidden(AccountIntention.SignOut);

            var sessionId = identityProvider.Current.SessionId;
            await storage.RemoveSession(sessionId, cancellationToken);
        }
    }
}
