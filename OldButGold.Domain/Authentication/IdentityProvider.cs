namespace OldButGold.Domain.Authentication
{
    public class IdentityProvider : IIdentityProvider
    {
        public IIdentity Current => new User(Guid.Parse("0f76b85b-13ed-4edf-ab23-f320d0150914"));
    }
}
