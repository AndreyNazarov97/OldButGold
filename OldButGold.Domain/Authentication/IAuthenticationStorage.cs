using OldButGold.Domain.UseCases.SignIn;

namespace OldButGold.Domain.Authentication
{
    public interface IAuthenticationStorage
    {
        Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken);
    }
}
