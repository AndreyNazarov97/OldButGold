using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.UseCases.SignOn
{
    public interface ISignOnUseCase
    {
        Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken);
    }
}
