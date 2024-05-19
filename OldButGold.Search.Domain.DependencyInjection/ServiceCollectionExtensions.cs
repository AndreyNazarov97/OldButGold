using Microsoft.Extensions.DependencyInjection;
using OldButGold.Search.Domain.Models;

namespace OldButGold.Search.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearchDomain(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg
               .RegisterServicesFromAssemblyContaining<SearchEntity>());

            return services;
        }
    }
}
