using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Authorization
{
    public interface IIntentionResolver
    {

    }

    public interface IIntentionResolver<in Tintention> : IIntentionResolver
    {
        bool isAllowed(IIdentity subject, Tintention intention);
    }
}
