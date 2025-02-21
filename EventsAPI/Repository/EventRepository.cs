using EventsAPI.Data;
using EventsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Repository
{
    public interface IEventRepository : IRepository<Event>
    {
        new Task<Event?> GetByIdAsync(int id);
        IEnumerable<Event> GetAllEventsWithFilters(GetEventsModel model);
    }
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationContext context) : base(context) { }
        new public async Task<Event?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
        }
        public IEnumerable<Event> GetAllEventsWithFilters(GetEventsModel model)
        {
            var query = _dbSet.AsQueryable();

            if (model.Name != null)
            {
                query = query.Where(e => e.Name.Contains(model.Name));
            }

            if (model.DateFrom.HasValue)
            {
                query = query.Where(e => e.EventDateTime >= model.DateFrom);
            }

            if (model.DateTo.HasValue)
            {
                query = query.Where(e => e.EventDateTime <= model.DateTo);
            }

            if (model.Category.HasValue && model.Category.Value != EventCategories.Unspecified)
            {
                query = query.Where(e => e.Category == model.Category);
            }

            return query.AsEnumerable();
        }
    }
}
