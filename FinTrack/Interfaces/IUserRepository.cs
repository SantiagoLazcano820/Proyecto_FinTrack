using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailDapperAsync(string email);
        Task<IEnumerable<User>> GetAllUsersDapperAsync();
        Task<User> GetUserByIdDapperAsync(int id);
    }
}
