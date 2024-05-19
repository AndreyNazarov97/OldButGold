using Microsoft.Extensions.DependencyInjection;
using OldButGold.Search.Domain.UseCases.Index;
using OldButGold.Search.Domain.UseCases.Search;
using OldButGold.Search.Storage.Entities;
using OldButGold.Search.Storage.Storages;
using OpenSearch.Client;

namespace OldButGold.Search.Storage.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearchStorage(this IServiceCollection services, string connectionString)
        {
            services
                .AddScoped<IIndexStorage, IndexStorage>()
                .AddScoped<ISearchStorage, SearchStorage>();

            services.AddSingleton<IOpenSearchClient>(new OpenSearchClient(new Uri(connectionString))
            {
                ConnectionSettings = 
                {
                    DefaultIndices = { [typeof(SearchEntity)]= "obg-search-v1"}
                }
            });
                
            return services;
        }
    }
}
