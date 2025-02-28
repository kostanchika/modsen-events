using EventsAPI.DAL.Entities;

namespace EventsAPI.DAL.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        new Task<Event?> GetByIdAsync(int id, CancellationToken ct);
        IEnumerable<Event> GetAllEventsWithFilters(
            int page,
            int pageSize,
            string? Name,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? Location,
            EventCategories? category
        );
        Task<int> GetTotalPagesAsync(
            int pageSize,
            string? Name,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? Location,
            EventCategories? category,
            CancellationToken ct = default
        );
    }
}
