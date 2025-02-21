using EventsAPI.Data;
using EventsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Repository
{
    public interface IEventRepository : IRepository<Event>
    {
        new Task<Event?> GetByIdAsync(int id);
        IEnumerable<Event> GetAllEventsWithFilters(GetEventsModel model);
        int GetTotalPages(int pageSize);
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
            if (model.Page <= 0 || model.PageSize <= 0)
            {
                throw new ArgumentException("Номер и размер страницы должны быть больше 0");
            }

            if (model.PageSize > 100)
            {
                throw new ArgumentException("Размер страницы не может быть больше 100");
            }

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

            return query.AsEnumerable().Skip((model.Page - 1) * model.PageSize).Take(model.PageSize);
        }

        public int GetTotalPages(int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("Размер страницы должен быть больше 0");
            }

            if (pageSize > 100)
            {
                throw new ArgumentException("Размер страницы не может быть больше 100");
            }

            int totalItems = _dbSet.Count();
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }

    }
}
