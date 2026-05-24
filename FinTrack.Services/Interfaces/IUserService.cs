using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters);
        Task<User> GetUserByIdAsync(int id);
        Task<ResponseData> GetAllUsersDapperAsync(UserQueryFilter filters);
        Task InsertUser(User user);
        void UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
