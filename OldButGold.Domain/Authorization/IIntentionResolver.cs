using OldButGold.Forums.Domain.Authentication;

namespace OldButGold.Forums.Domain.Authorization
{
    public interface IIntentionResolver
    {

    }

    public interface IIntentionResolver<in Tintention> : IIntentionResolver
    {
        bool IsAllowed(IIdentity subject, Tintention intention);
    }
    public interface IIntentionResolver<in TIntention, in TObject> : IIntentionResolver
    {
        bool IsAllowed(IIdentity subject, TIntention intention, TObject target);
    }
}
