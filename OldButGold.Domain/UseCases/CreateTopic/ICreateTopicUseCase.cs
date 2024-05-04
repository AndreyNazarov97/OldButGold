using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        Task<Topic> Execute(CreateTopicCommand command ,CancellationToken cancellationToken);
    }
}
