
using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.UseCases.SignIn
{
    public interface ISignInUseCase
    {

        Task<(IIdentity identity, string token)> Execute(SignInCommand command, CancellationToken cancellationToken);
    }
}
