using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Chat.Infrastructure.FluentAPI;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.EF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=tcp:shchoholiev-chat-server.database.windows.net," +
                "1433;Initial Catalog=shchoholiev-chat-db;Persist Security Info=False;User " +
                "ID=shchoholiev;Password=27rhefoYvd20;MultipleActiveResultSets=False;" +
                "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ConnectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Connection> Connections { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
