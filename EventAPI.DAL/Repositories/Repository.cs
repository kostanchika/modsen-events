using EventsAPI.DAL.Data;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase, new()
    {
        private readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = new T { Id = id };

            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);

            return entity;

        }
    }
}
