using EventsAPI.DAL.Entities;

namespace EventsAPI.DAL.Interfaces
{
    public interface IRepository<T> where T : EntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        Task<T> UpdateAsync(T entity, CancellationToken ct = default);
        Task<T?> DeleteAsync(int id, CancellationToken ct = default);
    }
}
