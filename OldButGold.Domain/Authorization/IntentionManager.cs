using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Authorization
{
    internal class IntentionManager(
        IEnumerable<IIntentionResolver> resolvers,
        IIdentityProvider identityProvider) : IIntentionManager
    {
        public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
        {
            var mathcingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
            return mathcingResolver?.isAllowed(identityProvider.Current, intention) ?? false;
        }
    }
}
