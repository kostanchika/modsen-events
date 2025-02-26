using EventsAPI.DAL.Entities;

namespace EventsAPI.Models
{
    public record class CreateEventModel(
        string Name,
        string Description,
        DateTime EventDateTime,
        string Location,
        EventCategories Category,
        int MaximumParticipants,
        IFormFile? Image
    );
}
