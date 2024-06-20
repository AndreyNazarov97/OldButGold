namespace OldButGold.Forums.Domain.UseCases.CreateForum
{
    public interface ICreateForumStorage : IStorage
    {
        Task<Models.Forum> CreateForum(string title, CancellationToken cancellationToken);
    }
}
