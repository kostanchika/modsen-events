using EventsAPI.Data;
using EventsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Repository
{
    public interface IEventRepository : IRepository<Event>
    {
        new Task<Event?> GetByIdAsync(int id);
    }
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationContext context) : base(context) { }
        new public async Task<Event?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
