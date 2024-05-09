namespace OldButGold.Domain.Authentication
{
    public interface IIdentityProvider 
    {
        IIdentity Current { get; set; }
    }
}
