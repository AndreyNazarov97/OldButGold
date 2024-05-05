using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.GetTopics
{
    public interface IGetTopicsUseCase
    {
        Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken);
    }
}
