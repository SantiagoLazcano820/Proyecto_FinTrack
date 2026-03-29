using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task InsertUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
    }
}
