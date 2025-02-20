using Microsoft.EntityFrameworkCore;
using EventsAPI.Models;

namespace EventsAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.BirthDateTime)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired();

                entity.HasIndex(e => e.Login)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });
        }
    }
}
