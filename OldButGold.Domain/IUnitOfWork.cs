namespace OldButGold.Domain
{
    public interface IUnitOfWork
    {
        Task<IUnitOfWorkScope> CreateScope(CancellationToken cancellationToken);


    }

    public interface IUnitOfWorkScope : IAsyncDisposable
    {
        TStorage GetStorage<TStorage>() where TStorage : IStorage;

        Task Commit(CancellationToken cancellationToken);
    }

    public interface IStorage;

}
