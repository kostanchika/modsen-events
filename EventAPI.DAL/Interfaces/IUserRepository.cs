using EventsAPI.DAL.Entities;

namespace EventsAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByLoginAsync(string login, CancellationToken ct = default);
        Task<User?> GetByLoginIncludeEventsAsync(string login, CancellationToken ct = default);
    }
}
