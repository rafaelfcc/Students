using Escola.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Escola.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Password).IsRequired().HasMaxLength(200);

                entity.HasIndex(x => x.Username).IsUnique();
            });

            CreateAdmin(modelBuilder);

            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Endereco).HasMaxLength(300);
                entity.Property(x => x.NomePai).HasMaxLength(200);
                entity.Property(x => x.NomeMae).HasMaxLength(200);
            });

            base.OnModelCreating(modelBuilder);
        }

        private static void CreateAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = -1,
                Username = "admin",
                Password = "AQAAAAIAAYagAAAAEJG4wxqJu95i2TBwY1b9j5+udStDH6zVfoA4IguRzABuoySb3c91iC0w3u/Si7H0nQ=="
            });
        }
    }
}
