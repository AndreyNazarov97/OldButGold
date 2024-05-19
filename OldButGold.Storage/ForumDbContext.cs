﻿using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Storage.Entities;

namespace OldButGold.Forums.Storage
{
    public class ForumDbContext(DbContextOptions<ForumDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<DomainEvent> DomainEvents { get; set; }

    }
}
