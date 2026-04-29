using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(UserQueryFilter filters);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersDapperAsync();
        Task<User> GetUserByIdDapperAsync(int id);
        Task InsertUser(User user);
        void UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<User> Authenticate(string email, string password);
    }
}
