using EventsAPI.DAL.Data;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }

        public async Task<User?> GetByLoginAsync(string login, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<User?> GetByLoginIncludeEventsAsync(string login, CancellationToken ct = default)
        {
            return await _dbSet.Include(u => u.Events).FirstOrDefaultAsync(u => u.Login == login, ct);
        }
    }
}
