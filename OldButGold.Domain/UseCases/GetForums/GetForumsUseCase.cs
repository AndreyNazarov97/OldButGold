using MediatR;

namespace OldButGold.Forums.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase(
        IGetForumsStorage storage) : IRequestHandler<GetForumsQuery, IEnumerable<Models.Forum>>
    {
        public async Task<IEnumerable<Models.Forum>> Handle(GetForumsQuery query, CancellationToken cancellationToken)
        {
            return await storage.GetForums(cancellationToken);
        }

    }
}
