using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Forums.Domain;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.DomainEvents;
using OldButGold.Forums.Domain.UseCases.CreateForum;
using OldButGold.Forums.Domain.UseCases.CreateTopic;
using OldButGold.Forums.Domain.UseCases.GetForums;
using OldButGold.Forums.Domain.UseCases.GetTopics;
using OldButGold.Forums.Domain.UseCases.SignIn;
using OldButGold.Forums.Domain.UseCases.SignOn;
using OldButGold.Forums.Domain.UseCases.SignOut;
using OldButGold.Forums.Storage.Storages;
using System.Reflection;

namespace OldButGold.Forums.Storage.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
        {
            services
                .AddScoped<IDomainEventStorage, DomainEventStorage>()
                .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
                .AddScoped<ICreateCommentStorage, CreateForumStorage>()
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
