using EventsAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsAPI.DAL.Data
{
    internal class UserSeedConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
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
                    RefreshToken = "_",
                    RefreshTokenExpiresAt = new DateTime(2025, 2, 24, 23, 0, 0).ToUniversalTime(),
                    Role = "Admin"
                }
            );
        }
    }
}
