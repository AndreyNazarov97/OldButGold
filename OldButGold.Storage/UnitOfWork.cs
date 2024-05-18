using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain;

namespace OldButGold.Storage
{
    internal class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
    {
        public async Task<IUnitOfWorkScope> CreateScope(CancellationToken cancellationToken)
        {
            var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            return new UnitOfWorkScope(scope, transaction);
        }
    }
    internal class UnitOfWorkScope(
        IServiceScope scope,
        IDbContextTransaction transaction) : IUnitOfWorkScope
    {
        public Task Commit(CancellationToken cancellationToken)
        {
            return transaction.CommitAsync(cancellationToken);
        }


        public TStorage GetStorage<TStorage>() where TStorage : IStorage
        {
            return scope.ServiceProvider.GetRequiredService<TStorage>();
        }

        public async ValueTask DisposeAsync()
        {
            if (scope is IAsyncDisposable scopeAsuncDisposable)
            {
                await scopeAsuncDisposable.DisposeAsync();
            }
            else
            {
                scope.Dispose();
            }
            await transaction.DisposeAsync();
        }
    }
}
