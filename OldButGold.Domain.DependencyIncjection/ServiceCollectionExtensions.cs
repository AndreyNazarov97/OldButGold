using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Domain.DependencyIncjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            services
                .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>();

            services
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIdentityProvider, IdentityProvider>();

            services.AddValidatorsFromAssemblyContaining<Forum>(includeInternalTypes: true);

            return services;
        }
    }
}
