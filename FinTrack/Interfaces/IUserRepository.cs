using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
