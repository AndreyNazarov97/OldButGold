using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.GetForums
{
    public interface IGetForumsStorage
    {
        Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
    }
}
