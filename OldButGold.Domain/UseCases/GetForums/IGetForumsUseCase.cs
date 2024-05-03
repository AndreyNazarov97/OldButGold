using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.GetForums
{
    public interface IGetForumsUseCase
    {
        Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken); 
    }


}
