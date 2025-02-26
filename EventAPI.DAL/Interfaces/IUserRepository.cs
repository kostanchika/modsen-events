using EventsAPI.DAL.Entities;

namespace EventsAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByLoginIncludeEventsAsync(string login);
    }
}
