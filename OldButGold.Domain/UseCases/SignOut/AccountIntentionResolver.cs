using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;

namespace OldButGold.Domain.UseCases.SignOut
{
    internal class AccountIntentionResolver : IIntentionResolver<AccountIntention>
    {
        public bool isAllowed(IIdentity subject, AccountIntention intention)
        {
            return intention switch
            {
                AccountIntention.SignOut => subject.isAuthenticated(),
                _ => false,
            };
        }
    }
}
