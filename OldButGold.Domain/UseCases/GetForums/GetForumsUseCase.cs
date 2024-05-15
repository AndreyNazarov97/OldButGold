using MediatR;
using Forum = OldButGold.Domain.Models.Forum;

namespace OldButGold.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase(
        IGetForumsStorage storage) : IRequestHandler<GetForumsQuery ,IEnumerable<Forum>>
    {
        public async Task<IEnumerable<Forum>> Handle(GetForumsQuery query ,CancellationToken cancellationToken)
        {
            return await storage.GetForums(cancellationToken);
        }

    }
}
