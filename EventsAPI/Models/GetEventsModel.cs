namespace EventsAPI.Models
{
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
