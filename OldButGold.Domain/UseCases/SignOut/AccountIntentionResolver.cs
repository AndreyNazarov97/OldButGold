using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;

namespace OldButGold.Forums.Domain.UseCases.SignOut
{
    internal class AccountIntentionResolver : IIntentionResolver<AccountIntention>
    {
        public bool IsAllowed(IIdentity subject, AccountIntention intention)
        {
            return intention switch
            {
                AccountIntention.SignOut => subject.isAuthenticated(),
                _ => false,
            };
        }
    }
}
