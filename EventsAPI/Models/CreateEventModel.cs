using EventsAPI.DAL.Entities;

namespace EventsAPI.Models
{
    public record class CreateEventModel(
        string? Name,
        string? Description,
        DateTime EventDateTime,
        EventCategories EventCategories,
        string? Location,
        int MaximumParticipants,
        IFormFile? Image
    );
}
