namespace EventsAPI.Models
{
    public record class ChangeEventModel(
        string Description,
        DateTime EventDateTime,
        string Location,
        EventCategories Category,
        int MaximumParticipants,
        IFormFile? Image
    );
}
