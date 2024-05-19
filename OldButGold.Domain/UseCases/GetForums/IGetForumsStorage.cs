namespace OldButGold.Forums.Domain.UseCases.GetForums
{
    public interface IGetForumsStorage
    {
        Task<IEnumerable<Models.Forum>> GetForums(CancellationToken cancellationToken);
    }
}
