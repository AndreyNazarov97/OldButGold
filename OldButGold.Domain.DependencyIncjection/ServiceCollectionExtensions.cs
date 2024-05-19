using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.Monitoring;
using OldButGold.Forums.Domain.UseCases;
using OldButGold.Forums.Domain.UseCases.CreateForum;
using OldButGold.Forums.Domain.UseCases.CreateTopic;

namespace OldButGold.Forums.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg
            .AddOpenBehavior(typeof(MonitoringPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>))
            .RegisterServicesFromAssemblyContaining<Models.Forum>());

            services
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIntentionResolver, ForumIntentionResolver>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>();

            services
                .AddScoped<IIdentityProvider, IdentityProvider>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
                .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
                .AddScoped<IPasswordManager, PasswordManager>();

            services.AddValidatorsFromAssemblyContaining<Forum>(includeInternalTypes: true);

            services.AddSingleton<DomainMetrics>();

            return services;
        }
    }
}
