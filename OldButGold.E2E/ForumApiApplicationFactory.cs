﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OldButGold.Forums.Storage;
using System.Security.Cryptography;
using OldButGold.Forums.API;
using Testcontainers.PostgreSql;

namespace OldButGold.Forums.E2E
{
    public class ForumApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer dbContainer = new PostgreSqlBuilder().Build();
        public ForumApiApplicationFactory()
        {

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:Postgres"] = dbContainer.GetConnectionString(),
                    ["Authentication:Base64Key"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                })
                .Build();

            builder.UseConfiguration(configuration);

            base.ConfigureWebHost(builder);
        }
        public async Task InitializeAsync()
        {
            await dbContainer.StartAsync();
            var forumDbContext = new ForumDbContext(new DbContextOptionsBuilder<ForumDbContext>()
                .UseNpgsql(dbContainer.GetConnectionString()).Options);
            await forumDbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await dbContainer.DisposeAsync();
        }
    }
}