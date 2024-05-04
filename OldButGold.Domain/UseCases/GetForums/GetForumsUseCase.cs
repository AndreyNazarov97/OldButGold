using Forum = OldButGold.Domain.Models.Forum;

namespace OldButGold.Domain.UseCases.GetForums
{
    public class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly IGetForumsStorage storage;

        public GetForumsUseCase(
            IGetForumsStorage storage)
        {
            this.storage = storage;
        }

        public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
        {
            return await storage.GetForums(cancellationToken);
        }
    }
}
