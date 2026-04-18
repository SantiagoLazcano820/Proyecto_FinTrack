using FinTrack.Core.Entities;

namespace FinTrack.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task InsertUser(User user);
        Task UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<User> Authenticate(string email, string password);
    }
}
