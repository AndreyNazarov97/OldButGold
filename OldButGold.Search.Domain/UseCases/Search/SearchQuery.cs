using MediatR;
using OldButGold.Search.Domain.Models;

namespace OldButGold.Search.Domain.UseCases.Search
{
    public record SearchQuery(string Query) : IRequest<(IEnumerable<SearchResult> resources, int totalCount)>;
}
