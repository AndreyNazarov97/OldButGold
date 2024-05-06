﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Domain.UseCases.GetTopics;
using OldButGold.Storage.Storages;

namespace OldButGold.Storage.DependencyIncjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this  IServiceCollection services, string dbConnectionString)
        {
            services
                .AddScoped<IGetForumsStorage, GetForumStorage>()
                .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                .AddScoped<IGuidFactory, GuidFactory>()
                .AddScoped<IMomentProvider, MomentProvider>();

            services.AddDbContextPool<ForumDbContext>(options =>
                options.UseNpgsql(dbConnectionString));

            return services;
        }

    }
}