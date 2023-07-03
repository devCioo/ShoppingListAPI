using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;

namespace ShoppingListAPI
{
    public class ShoppingListDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=ShoppingListDb;Trusted_Connection=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserData> UsersDatas { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<UserData>()
                .Property(ud => ud.Email)
                .IsRequired();

            modelBuilder.Entity<ShoppingList>()
                .Property(sl => sl.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Item>()
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(70);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
