using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Domain.UseCases.GetTopics;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Domain.UseCases.SignOn;
using OldButGold.Domain.UseCases.SignOut;

namespace OldButGold.Domain.DependencyIncjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            services
                .AddScoped<ICreateForumUseCase, CreateForumUseCase>()
                .AddScoped<IIntentionResolver, ForumIntentionResolver>()
                .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
                .AddScoped<ISignInUseCase, SignInUseCase>()
                .AddScoped<ISignOnUseCase, SignOnUseCase>()
                .AddScoped<ISignOutUseCase, SignOutUseCase>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>();

            services
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIdentityProvider, IdentityProvider>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
                .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
                .AddScoped<IPasswordManager, PasswordManager>();
            
            services.AddValidatorsFromAssemblyContaining<Forum>(includeInternalTypes: true);

            

            return services;
        }
    }
}
