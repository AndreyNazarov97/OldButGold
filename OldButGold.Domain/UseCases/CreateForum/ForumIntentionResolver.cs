using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;

namespace OldButGold.Forums.Domain.UseCases.CreateForum
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
