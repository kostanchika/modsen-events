using EventsAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsAPI.DAL.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
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

            entity.HasData(
                new User
                {
                    Id = 1,
                    Login = "admin",
                    PasswordHash = "$2a$11$fR8mRh4qNDDrAeao7y7RQezOoFsfG3h7nudLj/Gxqt.faTukK4bIu", //admin123
                    Name = "Администратор",
                    LastName = "Главный",
                    BirthDateTime = new DateTime(2005, 3, 1, 3, 0, 0).ToUniversalTime(),
                    Email = "admin@gmail.com",
                    Events = [],
                    RefreshToken = "_",
                    RefreshTokenExpiresAt = new DateTime(2025, 2, 24, 23, 0, 0).ToUniversalTime(),
                    Role = "Admin"
                }
            );
        }
    }
}
