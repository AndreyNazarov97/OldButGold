using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.UseCases;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Domain.UseCases.GetTopics;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Domain.UseCases.SignOn;
using OldButGold.Domain.UseCases.SignOut;
using OldButGold.Storage.Storages;
using System.Reflection;

namespace OldButGold.Storage.DependencyIncjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this  IServiceCollection services, string dbConnectionString)
        {
            services
                .AddScoped<IDomainEventStorage, DomainEventStorage>()
                .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
                .AddScoped<ICreateForumStorage, CreateForumStorage>()
                .AddScoped<IGetForumsStorage, GetForumStorage>()
                .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                .AddScoped<ISignInStorage, SignInStorage>()
                .AddScoped<ISignOnStorage, SignOnStorage>()
                .AddScoped<ISignOutStorage, SignOutStorage>()
                .AddScoped<IGuidFactory, GuidFactory>()
                .AddScoped<IMomentProvider, MomentProvider>();

            services
                .AddScoped<IUnitOfWork>(sp => new UnitOfWork(sp));

            services.AddDbContextPool<ForumDbContext>(options =>
                options.UseNpgsql(dbConnectionString));

            services.AddMemoryCache();

            services.AddAutoMapper(config => config
                .AddMaps(Assembly.GetAssembly(typeof(ForumDbContext)))); 

            return services;
        }

    }
}
