using OldButGold.Search.Domain.Models;

namespace OldButGold.Search.Domain.UseCases.Search
{
    public interface ISearchStorage
    {
        Task<(IEnumerable<SearchResult> resources, int totalCount)> Search(string query, CancellationToken cancellation);
    }
}
