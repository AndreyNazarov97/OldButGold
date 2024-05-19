using MediatR;
using OldButGold.Search.Domain.Models;

namespace OldButGold.Search.Domain.UseCases.Search
{
    internal class SearchUseCase(
        ISearchStorage storage) : IRequestHandler<SearchQuery, (IEnumerable<SearchResult> resources, int totalCount)>
    {
        public Task<(IEnumerable<SearchResult> resources, int totalCount)> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            return storage.Search(request.Query, cancellationToken);
        }
    }
}
