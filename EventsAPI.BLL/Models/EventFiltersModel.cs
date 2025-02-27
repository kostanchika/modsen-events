using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Models
{
    public record class EventFiltersModel(
        string? Name,
        DateTime? DateFrom,
        DateTime? DateTo,
        string? Location,
        EventCategories? Category,
        int Page,
        int PageSize
    );
}
