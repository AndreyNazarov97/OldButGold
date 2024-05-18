using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public class ForumIntentionResolver : IIntentionResolver<ForumIntention>
    {
        public bool IsAllowed(IIdentity subject, ForumIntention intention)
        {
            return intention switch
            {
                ForumIntention.Create => subject.isAuthenticated(),
                _ => false,
            };
        }
    }
}
