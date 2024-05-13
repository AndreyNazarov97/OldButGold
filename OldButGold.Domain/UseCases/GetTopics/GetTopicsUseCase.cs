using FluentValidation;
using MediatR;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Domain.UseCases.GetTopics
{
    internal class GetTopicsUseCase : 
        IRequestHandler<GetTopicsQuery, (IEnumerable<Topic> resources, int totalCount)>
    {
        private readonly IValidator<GetTopicsQuery> validator;
        private readonly IGetForumsStorage getForumsStorage;
        private readonly IGetTopicsStorage storage;

        public GetTopicsUseCase(
            IValidator<GetTopicsQuery> validator,
            IGetForumsStorage getForumsStorage,
            IGetTopicsStorage storage)
        {
            this.validator = validator;
            this.getForumsStorage = getForumsStorage;
            this.storage = storage;
        }

        public async Task<(IEnumerable<Topic> resources, int totalCount)> Handle(GetTopicsQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query, cancellationToken);
            await getForumsStorage.ThrowIfFormNotExist(query.ForumId, cancellationToken);
            return await storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
        }
    }
}
