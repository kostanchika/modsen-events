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

    public record class ChangeEventModel(
        string Description,
        DateTime EventDateTime,
        string Location,
        EventCategories Category,
        int MaximumParticipants,
        IFormFile? Image
    );

    public record class GetEventsModel(
        string? Name,
        DateTime? DateFrom,
        DateTime? DateTo,
        string? Location,
        EventCategories? Category,
        int Page,
        int PageSize
    );

    public class GetEventsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Location { get; set; }
        public EventCategories Category { get; set; }
        public int MaximumParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public string ImagePath { get; set; }
    }

}
