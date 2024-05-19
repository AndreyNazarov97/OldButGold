namespace OldButGold.Forums.Domain.Authentication
{
    public interface IIdentityProvider
    {
        IIdentity Current { get; set; }
    }
}
