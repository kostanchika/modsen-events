using EventsAPI.Data;
using EventsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByLoginIncludeEventsAsync(string login);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User?> GetByLoginIncludeEventsAsync(string login)
        {
            return await _dbSet.Include(u => u.Events).FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
