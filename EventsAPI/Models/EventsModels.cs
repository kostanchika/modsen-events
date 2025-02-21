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
}
