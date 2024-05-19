using FluentValidation;
using MediatR;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.GetForums;

namespace OldButGold.Forums.Domain.UseCases.GetTopics
{
    internal class GetTopicsUseCase(
        IValidator<GetTopicsQuery> validator,
        IGetForumsStorage getForumsStorage,
        IGetTopicsStorage storage) :
        IRequestHandler<GetTopicsQuery, (IEnumerable<Topic> resources, int totalCount)>
    {
        public async Task<(IEnumerable<Topic> resources, int totalCount)> Handle(GetTopicsQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query, cancellationToken);
            await getForumsStorage.ThrowIfFormNotExist(query.ForumId, cancellationToken);
            return await storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
        }
    }
}
