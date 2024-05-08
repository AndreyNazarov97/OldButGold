namespace OldButGold.Domain.Authentication
{
    public interface IAuthenticationService
    {
        Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);

    }
}
