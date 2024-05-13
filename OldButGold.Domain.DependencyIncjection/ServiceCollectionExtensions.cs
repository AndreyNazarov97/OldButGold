using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Models;
using OldButGold.Domain.Monitoring;
using OldButGold.Domain.UseCases;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Domain.UseCases.CreateTopic;

namespace OldButGold.Domain.DependencyIncjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg
            .AddOpenBehavior(typeof(MonitoringPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>))
            .RegisterServicesFromAssemblyContaining<Forum>());

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
