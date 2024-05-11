namespace OldButGold.Domain.UseCases.SignOut
{
    public interface ISignOutUseCase
    {
        Task Execute(SignOutCommand command, CancellationToken cancellation);
    }
}
