using EventsAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventsAPI.DAL.Data
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.EventDateTime)
                .IsRequired();

            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Category)
                .IsRequired();

            entity.Property(e => e.MaximumParticipants)
                .IsRequired();

            entity.Property(e => e.ImagePath)
                .HasDefaultValue("/images/default.png");

            entity.HasMany(e => e.Participants)
                .WithMany(u => u.Events)
                .UsingEntity(j => j.ToTable("EventsParticipants"));
        }
    }
}
