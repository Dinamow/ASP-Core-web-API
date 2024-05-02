using loginService.Models;
using Microsoft.EntityFrameworkCore;

namespace loginService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.HasKey(u => u.Id);
                entity.HasAlternateKey(u => u.Email);
                entity.HasAlternateKey(u => u.Username);
            });
            modelBuilder.Entity<UserConnection>(entity =>
            {
                entity.HasKey(uc => new { uc.User1Id, uc.User2Id });

                entity.HasOne(uc => uc.User1)
                      .WithMany()
                      .HasForeignKey(uc => uc.User1Id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uc => uc.User2)
                      .WithMany()
                      .HasForeignKey(uc => uc.User2Id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static User CreateDefaultUser()
        {
            var user = new User()
            {
                Username = "DINAMOW",
                Email = "meemoo102039@gmail.com",
                Phone = "+201208677550",
                Role = "Admin",
                Gender = "male"
            };
            user.setPassword("Aadmin123");
            return user;
        }

        // Seed the database
        public void SeedData()
        {
            var user = CreateDefaultUser();
            Users.Add(user);
            SaveChanges();
        }
    }
}
