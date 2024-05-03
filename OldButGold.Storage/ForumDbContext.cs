using Microsoft.EntityFrameworkCore;

namespace OldButGold.Storage
{
    public class ForumDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Comment> Comments {  get; set; }    



    }
}
