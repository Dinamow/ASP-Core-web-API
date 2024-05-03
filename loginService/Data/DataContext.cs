using loginService.Models;
using Microsoft.EntityFrameworkCore;

namespace loginService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key for Connection entity
            modelBuilder.Entity<Connection>()
                .HasKey(c => new { c.SenderUserId, c.ReceiverUserId });

            // Configure one-to-many relationship between User and Connection
            modelBuilder.Entity<User>()
                .HasMany(u => u.SentConnections)
                .WithOne(c => c.SenderUser)
                .HasForeignKey(c => c.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedConnections)
                .WithOne(c => c.ReceiverUser)
                .HasForeignKey(c => c.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
