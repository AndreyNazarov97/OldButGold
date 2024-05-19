
using MediatR;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;

namespace OldButGold.Forums.Domain.UseCases.SignOut
{
    internal class SignOutUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ISignOutStorage storage) : IRequestHandler<SignOutCommand>
    {
        public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            intentionManager.ThrowIfForbidden(AccountIntention.SignOut);

            var sessionId = identityProvider.Current.SessionId;
            await storage.RemoveSession(sessionId, cancellationToken);
        }
    }
}
